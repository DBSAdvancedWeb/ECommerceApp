import pyodbc
import csv
import re
import uuid
import random


def clean_string(str):
    # Remove special characters and symbols
    cleaned_str = re.sub(r'[^\w\s]', '', str)
    return cleaned_str    

def generate_random_price():
    # Generate a random price within the specified range
    return round(random.uniform(6.99, 22.00), 2)

def import_data_from_csv(csv_file, server, database, username, password):
    conn = pyodbc.connect(
        f'DRIVER=ODBC Driver 17 for SQL Server;'
        f'SERVER={server};'
        f'DATABASE={database};'
        f'UID={username};'
        f'PWD={password}'
    )
    cursor = conn.cursor()

    with open(csv_file, 'r', encoding='utf-8') as file:
        csv_reader = csv.reader(file)
        next(csv_reader)  # Skip header row
        for row in csv_reader:
            productId = uuid.uuid4();
            try:
                title = row[1]
                author = row[2]
                year = int(row[3]) if row[3] != '0' else 1990
                publisher = row[4]
                print(f"Importing book: {title}")
                #ISBN,Title,Author,Year,Publisher,ImageSmall,ImageMedium,Image
                cursor.execute('''
                    INSERT INTO Products (Id, Discriminator, Name, Description, Category, ImageSmall, ImageMedium, ImageLarge, Price, DateAdded, ISBN, Author, Year, Publisher)
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE(), ?, ?, ?, ?)          
                ''', (productId, 'Book', title, "", "book", row[5], row[6], row[7], generate_random_price(), row[0], author, year, publisher))
            except Exception as e:
                print(f"Failed to insert record {row[0]}: {e}")
                continue
    conn.commit()
    conn.close()

def main():
    server = 'localhost'
    database = 'ProductDB'
    username = 'sa'
    password = 'P@ssw0rd!'
    csv_file = './csv/Books.csv'

    import_data_from_csv(csv_file, server, database, username, password)

if __name__ == "__main__":
    main()