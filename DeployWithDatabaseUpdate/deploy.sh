env="$1"

if [ -z "$env" ]; then
    env="stage"
fi

echo "deploy $env"

# echo "get latest code"
# git pull origin

echo "build project"
docker build -f your-project/Dockerfile -t your-private-registry-address/your-project:latest .

echo "push image"
docker push your-private-registry-address/your-project:latest

echo "deploy configmap"
kubectl apply -f "./k8s/k8s-cm-$env.yaml"

echo "deploy ingress"
kubectl apply -f "k8s/k8s-lb-$env.yaml"

echo "update database"
if [ -n "`kubectl get pods --selector=job-name=your-project-updatedb`" ]; then
    kubectl delete -f ./k8s/k8s-update-db.yaml --namespace=default
fi
kubectl create -f ./k8s/k8s-update-db.yaml --namespace=default

updateDbSucceed=false

for i in {1..20}
do
    echo "wait database update result $i"
    if [ "Succeeded" != "`kubectl get pods --selector=job-name=your-project-updatedb --output=jsonpath={.items..status.phase}`" ]; then
        sleep 2s
    else
        updateDbSucceed=true;
        break;
    fi
done

if [ false == $updateDbSucceed ]; then
    echo "update database failed, exit"
    exit -1
fi

echo "deploy services"
if [ -n "`kubectl get deployment your-project`" ]; then
    kubectl patch -f ./k8s/k8s.yaml --namespace=default -p "{\"spec\":{\"template\":{\"metadata\":{\"labels\":{\"date\":\"`date +'%s'`\"}}}}}"
else
    kubectl apply -f ./k8s/k8s.yaml --namespace=default
fi
