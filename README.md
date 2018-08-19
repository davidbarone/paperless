# paperless
Demo application for recording checklist data for a manufacturing environment.

# installation
The paperless source comes in 3 parts:
- paperless-db: database script (SQL Server 2016)
- paperless-api: paperless api project (.NET Core 2.0 / Visual Studio Code)
- paperless-ui: paperles UI (SPA application using Vue)

# tools
In order to build and run this solution, you'll need the following:
- SQL Server 2016 instance
- .NET code 2.0
- Node (I used v8.11.2)
- NPM (I used 5.6.0)

# ide
The IDE used for all development (other than SQL Server development) was Visual Studio Code. This excellent IDE supports .NET Core application and UI/Vue projects. The following extensions are highly recommended:
- C# (OmniSharp): C# Syntax highlighting / intellisence
- EditorConfig for VS Code: Editor config support for VS Code
- ESLint: Javascript linter
- Vetur: Vue tooling

# build
To build this solution, the following steps should be done:
- Install the paperless database. Run the paperless.sql script. This will create a database called 'Paperless' on the target SQL Server 2016 instance.
- Open the paperless-api project (Go to the folder in Explorer, and run the VS Code editor). Edit the PaperlessController class in the Controllers folder, and set the connection string to the appropriate value.
- Start / run the project.
- Open the paperless-ui project. Edit the .env file, and set the VUE_APP_API_ROOT environment variable to the root path of the API application.
- Run 'npm install' to install all the dependencies.
- Run 'npm run serve' to start the application.
- The application will be served on http://localhost:8080

# tour of application
This application is a simple question/answer form-builder suitable for creating checklists. A typical example of this is the generation of checklists for a manufuacturing line. This application contains 2 sections:
- Design-time page
- Run-time page
The design time page enables administrations to maintain forms. New forms can be generated with form content. Forms can contain individual questions and also tabular information. The application is able to render moderately complex forms at the moment. Forms are built into a hierarchy:
- Form – Represents a complete form
- Section – Represents a section of a form
- Question – Represents an individual question in a section
Several types of question are supported including:
- Checkbox (completion of task) 
- Yes/No
- Number, text, date, time
- Memo / Multiline text
- Picklist
Once a form library has been created, an instance of a set of forms (called a 'pack') can be created. A pack is simply an instance of a set of forms for a specific date, manufacturing line and material number (SKU).

Once the pack has been created, users can enter the run-time area of the application and start recording the answers. Questions also support targets. These targets can have RAG conditional formatting applied to give operators immediate feedback on the values being entered.