SHOW wal_level;

SELECT version();

SELECT * FROM pg_extension;

SELECT * FROM pg_available_extensions


pg_recvlogical -h localhost -p 5432 -d elevate_recruit -U elevate_recruit_user --slot test_slot --create-slot -P wal2json

elevate


pg_recvlogical -h localhost -p 5432 -d elevate_recruit -U elevate_recruit_user --slot test_slot --start -o pretty-print=1 -f -

pg_recvlogical  -h localhost -p 5432 -d elevate_recruit -U elevate_recruit_user --slot test_slot --drop-slot


docker postgres

"Mounts": [
		{
			"Type": "volume",
			"Name": "postgres-data",
			"Source": "/var/lib/docker/volumes/postgres-data/_data",
			"Destination": "/var/lib/postgresql/data",
			"Driver": "local",
			"Mode": "z",
			"RW": true,
			"Propagation": ""
		}
	],

note:

you need to set a DNS name in KAFKA_ADVERTISED_LISTENERS environment variable that can be resolved
on all hosts, e.g. elevate.kafka.local
if using docker-compose its in the compose file

https://stackoverflow.com/questions/54443640/kafka-consumer-no-entry-found-for-connection

when running connect as a container make sure it can resolve the broker DNS name
e.g.
http://jasani.org/posts/docker-now-supports-adding-host-mappings-2014-11-19/index.html


https://stackoverflow.com/questions/48947250/kafka-connect-how-to-delete-a-connector

DELETE http://192.168.1.160:8083/connectors/workforce-connector

POST http://192.168.1.160:8083/connectors/

{
  "name": "workforce-connector",
  "config": {
    "connector.class": "io.debezium.connector.postgresql.PostgresConnector",
    "database.hostname": "192.168.1.160",
    "database.port": "5432",
    "database.user": "elevate_recruit_user",
    "database.password": "elevate",
    "database.dbname" : "elevate_recruit",
    "database.server.name": "workforce",
    "plugin.name": "wal2json",
    "schema.whitelist": "recruit",
    "table.whitelist": "recruit.candidate_rates"
  }
}

"plugin.name":"wal2json_rds"



docker run -it --rm --name elevate_connect \
-p 8083:8083 \
-e GROUP_ID=1 \
-e BOOTSTRAP_SERVERS=elevate.kafka.local:9092 \
-e CONFIG_STORAGE_TOPIC=elevate_connect_configs \
-e OFFSET_STORAGE_TOPIC=elevate_connect_offsets \
-e STATUS_STORAGE_TOPIC=elevate_connect_statuses \
--add-host elevate.kafka.local:192.168.1.160 \
debezium/connect:0.9

