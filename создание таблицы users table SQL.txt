use test
CREATE TABLE users(id_user int primary key identity(1,1),
login_user varchar(50) not null,
password_user varchar(50) not null,
is_admin bit)

insert into users(login_user,password_user,is_admin) values ('admin','admin',1)
