# NuGenee API Deployment Guide (ASP Monster)

This guide provides steps to deploy the NuGenee API to [ASP Monster](https://www.aspmonster.com/) or similar ASP.NET hosting providers.

## 1. Automated Deployment (GitHub WebDeploy) - [RECOMMENDED]

WebDeploy is faster and more reliable than FTP. 

1.  **Download Publish Profile**: 
    - Log in to your **ASP Monster Control Panel**.
    - Navigate to your Website/API settings.
    - Look for a button or link that says **"Download Publish Profile"** or **"WebDeploy Settings"**. This will download a `.publishsettings` file (it's an XML file).
2.  **Add GitHub Secret**: 
    - Go to your repository on GitHub.
    - Click **Settings** > **Secrets and variables** > **Actions**.
    - Add a new Repository secret named: `PUBLISH_PROFILE_XML`.
    - Open the downloaded `.publishsettings` file in Notepad, copy everything, and paste it into the GitHub secret value.
3.  **Push to main**: Once the secret is added, every push to `main` will trigger a WebDeploy automatically.
    - The workflow file is at `.github/workflows/deploy.yml`.

## 2. Manual Publish (Alternative)

## 2. Configure Production Database

1.  Log in to your **ASP Monster Control Panel**.
2.  Create a new **MSSQL Database**.
3.  Note down the **Connection String** provided.
4.  Open `publish/appsettings.json` (or `appsettings.Production.json`) and update the `DefaultConnection` with your server details:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=your_db;User Id=your_user;Password=your_pass;Encrypt=True;TrustServerCertificate=False;"
}
```

## 3. Upload Files

1.  Use an FTP client (like FileZilla) or the ASP Monster File Manager.
2.  Upload the contents of the `./publish` folder to the `wwwroot` directory of your site.

## 4. Set Environment Variable (Crucial)

To ensure the API runs in production mode (using production CORS and security settings):

1.  In the ASP Monster panel, look for **ASP.NET Core settings** or **Environment Variables**.
2.  Add a new variable:
    *   **Name**: `ASPNETCORE_ENVIRONMENT`
    *   **Value**: `Production`

## 5. First Run & Seeding

The API is configured to automatically create the database tables on the first run using `EnsureCreatedAsync()`. 
- It will also seed the initial admin account:
    - **Email**: `admin@nugenee.com`
    - **Password**: `Admin123!`

> [!WARNING]
> Please change the admin password immediately after your first login via the API.

## 6. Accessing Swagger (Optional)

If you need Swagger in production for testing, edit `Program.cs` and uncomment these lines before publishing:

```csharp
// app.UseSwagger();
// app.UseSwaggerUI();
```

## 7. Troubleshooting

- **500 Error**: Check if the database connection string is correct and if the SQL user has `db_owner` permissions.
- **CORS Error**: Ensure the `ProductionPolicy` in `Program.cs` matches your frontend domain.
