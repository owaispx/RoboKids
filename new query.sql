create table users(id int primary key identity,
firstname varchar (100) not null,
lastname varchar (100) not null,
email varchar (100) not null,
password varchar (100) not null,
crate_at datetime not null default current_timestamp
)
insert into users(firstname,lastname,email,password)
values ('owais','khan','khanowais51924@gmail.com','owais@12345')

select*from users