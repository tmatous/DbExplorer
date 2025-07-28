# DbExplorer

DbExplorer is a tool for exploring the structure and data of your databases. It uses standard .NET database drivers, so multiple databases are supported including SQL Server, Oracle, MySQL, and Access (32-bit version).

DbExplorer requires only the .NET Framework 4. Just download the .zip file, decompress it, and you're ready to go.

Major features:

- Store all your database connection strings
- Password encryption with a master password using AES
- Quickly view tables and columns with substring filtering
- Code generation or flat-file generation using Razor views
    + Generate files using:
        - Entire database model (structure)
        - Individual table model
        - Result recordset of a query
        - Individual rows of a query result
    + Sample CodeGen files included for:
        - POCO class generation from a table for CS/VB
        - CSV file from a recordset
        - SQL INSERT file from a recordset
        - HTML file from a recordset
        - XML file from a recordset
        - Text file for each row in a recordset
- Simple import/export with CSV files
- Tabbed query window with syntax highlighting
- Data editor for tabular data entry into your tables and queries