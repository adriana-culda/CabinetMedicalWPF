# Cabinet Medical - Sistem de Gestiune

Acest proiect reprezintă o aplicație desktop modernă, dezvoltată pentru eficientizarea activității unui cabinet medical. Aplicația permite gestionarea programărilor, a pacienților și a serviciilor medicale, oferind roluri diferențiate pentru **Admin, Medici și Asistenți**.



#### Am utilizat tehnologiile:
* **Framework:** WPF (Windows Presentation Foundation) cu arhitectura **MVVM** (Model-View-ViewModel).
* **Limbaj:** C# (.NET).
* **Baza de date:** Entity Framework Core (SQLite).
* **Interfață:** XAML pentru design, cu o structură modulară.
* **Comunitate/Tool-uri:** CommunityToolkit.Mvvm (pentru Data Binding și RelayCommands).

#### Cum funcționează:
* **Autentificare securizată:** Sistem de logare cu permisiuni bazate pe roluri (RBAC).
* **Dashboard Dinamic:** Vizualizare personalizată a programărilor în funcție de rol (Medicii văd programările proprii, Asistenții au o vedere de ansamblu).
* **Gestionarea Programărilor:** Adăugare, editare și anulare programări în timp real.
* **Catalog Servicii:** Administrarea serviciilor medicale și a prețurilor aferente.
* **Integrare Bază de Date:** Persistența datelor prin Entity Framework, asigurând integritatea informațiilor medicale.


Proiectul respectă principiul **Separation of Concerns**, având logica de business complet decuplată de interfața grafică, ceea ce face aplicația ușor de testat și de extins. Utilizarea `Data Binding` oferă o experiență fluidă și elimină necesitatea actualizării manuale a interfeței.

#### Autor
* **Adriana Culda**
