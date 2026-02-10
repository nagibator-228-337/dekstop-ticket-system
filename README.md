# Ticket Support System

A simple ticket support system where clients can create and track support requests without registration, and employees can manage tickets, communicate with clients, and coordinate work internally.

---

## Purpose

This application was developed as a practice project for **WPF**, **C#**, and fundamental application design principles.  
It demonstrates database interactions, UI binding, user authentication, and basic messaging functionality.

---

## Key Features

- **Ticket Management**
  - Create tickets without registration
  - Track tickets via a unique access code
  - Assign tickets to employees
  - Update ticket status

- **Messaging**
  - Clients can send messages within their tickets
  - Employees can leave internal notes
  - Messages are stored and displayed in chronological order

- **Employee Interface**
  - Login system with secure password hashing (SHA256)
  - View all tickets or filter to “My Tickets”
  - Sort and filter tickets by status or date
  - Double-click to view detailed ticket information

- **Client Interface**
  - Create a ticket with subject, description, and optional contact info
  - View ticket status and conversation using the unique access code

---

## Technology Stack

- **Language & Framework**: C# with WPF (.NET)
- **Database**: SQLite for local data storage
- **UI Components**: DataGrid, ListView, Grid, Buttons, TextBoxes, PasswordBox
- **Security**: Passwords stored as SHA256 hashes
- **Collections & Binding**: ObservableCollection for dynamic UI updates

---

## Architecture & Design

- **Database**
  - Tables: `Tickets`, `Employees`, `Messages`
  - Relationships: `Messages` are linked to `Tickets` via `TicketId`; `Tickets` can be assigned to `Employees`
  - Secure storage of employee credentials

- **UI**
  - **MainWindow**: central hub for creating tickets, searching tickets, and employee login
  - **Employee Page**: DataGrid listing tickets with sorting, filtering, and assignment options
  - **LoginWindow**: modal login window verifying credentials against the database
  - **Ticket Windows**: forms for creating tickets and viewing messages

- **Data Binding**
  - ObservableCollection used for real-time updates in DataGrid
  - Command patterns implemented via button click events for CRUD operations

---

## Usage

1. **Clients**
   - Click **Create Ticket** to open the ticket form
   - Fill out the **Subject**, **Description**, and optional contact info
   - After submission, receive a unique access code
   - Use **Find Ticket** to check ticket status and communicate

2. **Employees**
   - Click **Login**, enter credentials
   - View tickets in DataGrid
   - Switch between **All Tickets** and **My Tickets**
   - Double-click a ticket to view details and messages
   - Assign or reassign tickets, change status, and leave notes

---

## Installation

1. Download the ZIP file from the **Assets** section below.  
2. Extract all files to a folder of your choice.  
3. Double-click `DTS.exe` to run the application.

---
=
