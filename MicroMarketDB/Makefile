all: build run

build: remove
	docker build -t micro_market .

run: delete
	docker run --name=micro_market_container --restart=always -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=DBpass1434' -p 1434:1433 -d micro_market

remove: stop delete
	-docker image rm micro_market

delete: stop
	-docker rm micro_market_container

stop:
	-docker stop micro_market_container