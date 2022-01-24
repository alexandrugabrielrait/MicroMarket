for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P DBpass1434 -d master -i db-script.sql
    if [ $? -eq 0 ]
    then
        echo "setup.sql completed"
        break
    else
        echo "not ready yet..."
        sleep 1
    fi
done