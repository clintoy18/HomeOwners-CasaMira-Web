# HomeOwners Subdivision Management System

A **C# ASP.NET Core** web application powered by **Entity Framework Core** designed to streamline community operations within a subdivision or homeowners' association. This system empowers homeowners, staff, and administrators with an all-in-one digital platform for efficient, transparent, and community-centered management.

---

## Features

### User Management
- Role-based access: Homeowners, Subdivision Staff, Administrators  
- Profile management and secure authentication  
- Access control for different system functionalities  

### Announcements and Notifications
- Post subdivision news, urgent alerts, and community events  
- Automated email and/or SMS notifications *(not yet implemented)*  

### Billing and Payment Portal *(Optional)*
- View and track association dues and other billing statements  
- Secure online payments via integrated payment gateways  

### Facility Reservation
- Real-time reservation system for amenities (function halls, courts, pools, etc.)  
- Calendar view of availability and approved bookings  

### Service Request Management
- Submit, track, and update maintenance or security-related service requests *(not yet implemented)*  
- Staff view for request management and status updates  

### Document Management
- Downloadable forms, HOA guidelines, financial reports, and meeting minutes *(not yet implemented)*  
- Organized by categories for easy access  

### Community Forum
- Discussion boards for homeowners to collaborate, share ideas, and raise issues *(not yet implemented)*  
- Moderation tools for admins to manage posts  

### Emergency Contacts Directory
- List of emergency contacts for quick access  

### Event Calendar
- Centralized view of upcoming events, maintenance schedules, and activities *(in progress)*  

### Feedback and Complaints System
- Submit feedback or complaints with status tracking and admin response *(in progress)*  

### Contact Directory
- Directory of essential subdivision departments (HOA, Security, Maintenance, etc.)  

### Mobile-Friendly Design
- Fully responsive layout accessible across desktops, tablets, and smartphones  

### Reports and Analytics
- Dashboards and reports for service requests, billing data, and community activity  

### Polls and Surveys
- Create and publish polls/surveys for community engagement *(not yet implemented)*  

### Security and Privacy
- Encrypted user data and secure login system  
- Compliance with data privacy regulations (e.g., Data Privacy Act, GDPR)  

---

## Tech Stack

- Backend: ASP.NET Core Web API  
- Frontend: Razor Pages / MVC  
- Database: SQL Server + Entity Framework Core  
- Authentication: ASP.NET Identity  
- Notifications: SMTP (Email), Twilio or similar (SMS) *(not yet implemented)*  
- Deployment: IIS, Azure, or any cloud service  

---

## Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/clintoy18/HomeOwners-CasaMira-Web.git
   ```

2. **Update the connection string**  
   Edit `appsettings.json` with your SQL Server details.

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

---

## Notes

- This system is a work in progress. Some modules are not yet implemented.  
- Contributions and improvements are welcome.

---

## License

This project is open-source under the [MIT License](LICENSE).

---
