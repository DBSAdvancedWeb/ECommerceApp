import csv
import re
import uuid
import random
import sqlite3

def clean_string(s):
    # Remove special characters and symbols
    return re.sub(r'[^\w\s]', '', s)

def generate_random_price():
    # Generate a random price within the specified range
    return round(random.uniform(6.99, 22.00), 2)

def import_data_from_csv(csv_file, database_file):
    conn = sqlite3.connect(database_file)
    cursor = conn.cursor()

    with open(csv_file, 'r', encoding='utf-8') as file:
        csv_reader = csv.reader(file)
        next(csv_reader)  # Skip header row
        for row in csv_reader:
            productId = uuid.uuid4()
            try:
                title = row[1]
                author = row[2]
                year = int(row[3]) if row[3] != '0' else 1990
                publisher = row[4]
                print(f"Importing book: {title}")
                #ISBN,Title,Author,Year,Publisher,ImageSmall,ImageMedium,Image
                cursor.execute('''
                    INSERT INTO Products (Id, Name, Description, Category, ImageSmall, ImageMedium, ImageLarge, Price, DateAdded, ISBN, Author, Year, Publisher)
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, DATE('now'), ?, ?, ?, ?)
                ''', (productId, title, "", "book", row[5], row[6], row[7], generate_random_price(), row[0], author, year, publisher))
            except Exception as e:
                print(f"Failed to insert record {row[0]}: {e}")
                continue

    conn.commit()
    conn.close()

def main():
    database_file = 'ProductDB.sqlite'
    csv_file = './csv/Books.csv'

    import_data_from_csv(csv_file, database_file)

if __name__ == "__main__":
    main()
