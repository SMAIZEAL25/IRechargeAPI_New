Repository Purpose

# IRecharge API

## Overview
IRecharge is a C# Web API that facilitates the purchase of airtime, data, and electricity bill payments by integrating with an external service provider. The current version successfully supports airtime recharge, while data purchase and electricity bill payments are under development and will be available in upcoming releases.

## Features
- **Airtime Recharge** ✅ (Fully Functional)
- **Data Purchase** ⏳ (Under Development)
- **Electricity Bill Payment** ⏳ (Under Development)

## Tech Stack
- **Backend:** C# (.NET Web API)
- **Database:** SQL Server
- **Integration:** External API for transactions

## Installation
To set up the project locally, follow these steps:

### Prerequisites
Ensure you have the following installed:
- .NET SDK (latest version recommended)
- SQL Server
- Postman or any API testing tool (optional for testing API endpoints)

### Steps to Run
1. **Clone the Repository:**
   ```bash
   git clone https://github.com/yourusername/IRecharge.git
   cd IRecharge
   ```
2. **Set Up Database:**
   - Configure your database connection string in `appsettings.json`.
   - Run database migrations (if applicable).

3. **Run the Application:**
   ```bash
   dotnet run
   ```

4. **Test Endpoints:**
   - Use Postman or Swagger UI (`https://localhost:5001/swagger`) to test the available endpoints.

## API Endpoints
### Airtime Recharge (Working)
- **Endpoint:** `POST /api/recharge/airtime`
- **Request Body:**
  ```json
  {
    "phoneNumber": "08012345678",
    "amount": 1000
  }
  ```
- **Response:**
  ```json
  {
    "status": "success",
    "transactionId": "1234567890"
  }
  ```

### Data Purchase (Coming Soon)
- **Endpoint:** `POST /api/recharge/data`

### Electricity Bill Payment (Coming Soon)
- **Endpoint:** `POST /api/recharge/electricity`

## Roadmap
- [x] Implement Airtime Recharge
- [ ] Integrate Data Purchase API
- [ ] Implement Electricity Bill Payment Feature
- [ ] Enhance Error Handling and Logging
- [ ] Deploy to Production

## Contributing
Contributions are welcome! To contribute:
1. Fork the repository
2. Create a feature branch (`git checkout -b feature-name`)
3. Commit your changes (`git commit -m "Added new feature"`)
4. Push to the branch (`git push origin feature-name`)
5. Open a Pull Request

## License
This project is licensed under the MIT License.

## Contact
For any issues or feature requests, please create an issue on the [GitHub repository](https://github.com/yourusername/IRecharge).

---
### Note
The Data Purchase and Electricity Bill Payment features are currently under development and will be available in future versions.

