CREATE TABLE `all_data_types` (
id int PRIMARY KEY,
`c_varchar` VARCHAR( 20 )  ,
`c_nvarchar` VARCHAR( 20 ) CHARACTER SET utf16,
`c_tinyint` TINYINT  ,
`c_text` TEXT  ,
`c_date` DATE  ,
`c_smallint` SMALLINT  ,
`c_mediumint` MEDIUMINT  ,
`c_int` INT  ,
`c_bigint` BIGINT  ,
`c_float` FLOAT( 10, 2 )  ,
`c_double` DOUBLE  ,
`c_decimal` DECIMAL( 10, 2 )  ,
`c_datetime` DATETIME  ,
`c_timestamp` TIMESTAMP  ,
`c_time` TIME  ,
`c_year` YEAR  ,
`c_char` CHAR( 10 )  ,
`c_nchar` NCHAR( 10 )  ,
`c_tinyblob` TINYBLOB  ,
`c_tinytext` TINYTEXT  ,
`c_blob` BLOB  ,
`c_mediumblob` MEDIUMBLOB  ,
`c_mediumtext` MEDIUMTEXT  ,
`c_longblob` LONGBLOB  ,
`c_longtext` LONGTEXT  ,
`c_enum` ENUM( '1', '2', '3' )  ,
`c_set` SET( '1', '2', '3' )  ,
`c_bool` BOOL  ,
`c_binary` BINARY( 20 )  ,
`c_varbinary` VARBINARY( 20 ) 
)




CREATE TABLE parent_table (
id int PRIMARY KEY,
name VARCHAR(50) NOT NULL
)


CREATE TABLE child_table (
id int PRIMARY KEY,
parent_id int,
name VARCHAR(50) NOT NULL,
FOREIGN KEY (parent_id) REFERENCES parent_table (id)
)





create view parent_child_view as 
   select p.id as parent_id, p.name as parent_name, c.id as child_id, c.name as child_name
   from child_table c left join parent_table p on c.parent_id = p.id
