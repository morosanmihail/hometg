create table if not exists cards(uuid string, quantity int32, foilquantity int32, collection string, lastupdated datetime);
create table collection(id string, description string);

insert into collection (id, description) values ('Main', 'The main collection');
insert into collection (id, description) values ('Incoming', 'Track incoming card additions');
