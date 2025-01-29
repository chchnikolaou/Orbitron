# 📠 Orbitron - Διαδικτυακή Διαχείριση Κινητής Τηλεφωνίας
Η πρώτη πλατφόρμα που δημιουργώ με αυτό το Framework. Το Orbitron είναι μια web εφαρμογή διαχείρισης πελατών και έκδοσης λογαριασμών κινητής τηλεφωνίας, αναπτυγμένη με ASP.NET Core MVC. Σχεδιασμένη για πωλητές, πελάτες και διαχειριστές, η πλατφόρμα επιτρέπει την εύκολη διαχείριση λογαριασμών, προγραμμάτων και ιστορικού κλήσεων. Πρόκειται για μια ακαδημαϊκή εργασία και **δεν** αντιπροσωπεύει πραγματική εταιρεία ή εμπορική υπηρεσία.

## 📝 Χαρακτηριστικά
- Πελάτες: Προβολή και πληρωμή λογαριασμών, ιστορικό κλήσεων.
- Πωλητές: Καταχώρηση νέων πελατών, έκδοση λογαριασμών, αλλαγή προγραμμάτων.
- Διαχειριστές: Δημιουργία/τροποποίηση προγραμμάτων, διαχείριση πωλητών.

## 📡 Τεχνολογίες
- Backend: ASP.NET Core MVC
- Frontend: Razor Views, HTML, CSS
- Database: SQL Server
- Development Environment: Visual Studio 2022

## 📕 Οδηγίες
Είναι ένα απλό MVC C# ASP.NET Web Application. Για τη σύνδεση της βάσης πρέπει να δημιουργηθεί ένα SQL Database και να εισαχθεί το **Connection String** στο αρχείο appsettings.json στο πεδίο DefaultConnectionString
Παράδειγμα αρχείου appsettings.json:

````json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnectionString": "" // <-- Δώσε Connection String από το Data Connection
  }
}
````

## 📷 Εικόνες

![image](https://github.com/user-attachments/assets/80175e18-7852-4c60-9986-6582ae36ee27)


![image](https://github.com/user-attachments/assets/e94cee2b-c3fd-45d3-a832-ca88b5dfa579)
