# Use the official SQL Server 2019 Express base image from Microsoft
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=P@ssw0rd!

# Expose port 1433 for SQL Server
EXPOSE 1433

# Create a directory inside the container for your database files
RUN mkdir -p /var/opt/mssql/data

# Grant permissions for the SQL Server user
#RUN chown -R mssql:mssql /var/opt/mssql/data

# Set the working directory
WORKDIR /var/opt/mssql

# Copy any database scripts or backups to initialize the database
#COPY your_database_scripts_or_backups /var/opt/mssql/data

# Start SQL Server when the container starts
CMD [ "/opt/mssql/bin/sqlservr" ]