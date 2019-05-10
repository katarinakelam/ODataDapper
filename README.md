# ODataDapper
project combining odata web service protocol and dapper orm

## DBCreation 
- files that are in DBCreation folder can serve to create a database. The important thing to take notice of is the database structure,
the M:N relationship between Racuni and Stavke, and also the way of handling deletion of connected elements.
Example: 
  - deleting Stavka causes an entire row from Racun_Stavka table to dissappear
  - deleting Zaposlenik causes just the Zaposlenik foreign key in Racuni table to be set as null

## BaseRepository
- contains the main logic of database connection, serves as a kind of database context
- implements basic actions such as Query, QueryFirstOrDefault, Execute(for actions that Create, Update or Delete anything from the database)

## SQLQueryBuilder
- contains the logic that is responsible for translating OData query options to raw SQL - so far, options for FILTERING and ORDERING have been implemented
(The filter options with OData protocol are used only with collections)

## Console App
- represents an imitation of a OData client which, using common OData functions, calls the service and executes queries on the database.
