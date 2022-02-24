create database dolm_token collate utf8mb4_general_ci;

use dolm_token;

create table users(
    user_id int unsigned not null auto_increment,
    username varchar(100) not null,
    password varchar(300) not null,
    email varchar(150) null,
    constraint user_id_PK primary key(user_id));

insert into users values(null, "Luca", sha2("Hallo123!", 512), "l@gmail.com");
insert into users values(null, "Johannes", sha2("Hallo123!", 512), "j@gmail.com");
insert into users values(null, "Gabi", sha2("qwertzui", 512), "g@gmail.com");