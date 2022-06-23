select * from Client;
select * from Client_Trip;
select * from Trip;
select * from Country;
select * from Country_Trip;

insert into Client values ('testFN', 'testLN', 'test@test.com', '999888777', '98765432432');
insert into Trip values ('testName', 'testDesc', '2022-02-02', '2022-03-03', 20);
insert into Country values ('Poland');
insert into Client_Trip values (1, 1, '2022-01-01', '2022-01-01');
insert into Country_Trip values (1, 1);