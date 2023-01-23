create table if not exists cards(uuid string, quantity int32, foilquantity int32, collection string, lastupdated datetime);
create table collection(id string, description string);
create table if not exists incoming(uuid string, quantity int32, foilquantity int32, collection string, lastupdated datetime);
