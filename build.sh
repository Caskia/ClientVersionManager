#!/bin/bash 


prefix="hicoin"
declare -A apps
apps=([clientmanager-webapi]="./src/ClientVersionManager")
#登录docker仓库,如果需要
#docker login --username admin --password admin localhub.online
for app in $(echo ${!apps[*]})
do
    echo "$app : ${apps[$app]}"

    echo --------------开始编译 $app----------------

    docker build -t $prefix:$app.build-${BUILD_NUMBER} -f ${apps[$app]}/Dockerfile .

    echo --------------添加TAG----------------

    docker tag $prefix:$app.build-${BUILD_NUMBER} $REGISTRY_API/$prefix:$app.build-${BUILD_NUMBER}

    echo --------------推送到仓库----------------

    docker push $REGISTRY_API/$prefix:$app.build-${BUILD_NUMBER}

    echo --------------删除本地镜像----------------

    docker rmi $prefix:$app.build-${BUILD_NUMBER}

    docker rmi $REGISTRY_API/$prefix:$app.build-${BUILD_NUMBER}
done
