# Prophet 21 Business Rules Extension System

A sophisticated business rule extension framework for the Prophet 21 ERP system that implements intelligent warehouse sourcing, geographic-based location assignment, and real-time API integrations for distribution companies.

## Overview

This codebase extends Prophet 21's core functionality by implementing custom business rules for order entry processing. It provides automated decision-making for warehouse selection, inventory allocation, and pricing integration to optimize distribution operations.

## Core Features

### 🏭 Intelligent Warehouse Sourcing
- **Dynamic Location Assignment**: Automatically changes source warehouses based on shipping geography
- **Inventory Optimization**: Finds alternative warehouses when stock is insufficient at primary locations
- **Distance-based Selection**: Uses geographic calculations to minimize shipping costs

### 🌍 Geographic Business Logic
- **State-based Routing**: Routes orders to appropriate regional warehouses
- **ZIP Code Integration**: Calculates distances between warehouses and shipping destinations
- **Cross-state Fulfillment**: Handles scenarios where customer and warehouse are in different states

### 💰 Real-time Pricing Integration
- **API-driven Pricing**: Integrates with Prophet 21's REST API for current pricing
- **JWT Authentication**: Secure API communications
- **Automated Price Updates**: Ensures accuracy in quotes and orders

### 📊 Comprehensive Logging
- **Structured Logging**: Uses Serilog with multiple output destinations
- **Monitoring Integration**: Supports Seq, Elasticsearch, and SQL Server logging
- **Audit Trail**: Complete visibility into business rule execution

## Architecture

### Project Structure

```
Acme.P21/
├── Acme.Libraries/
│   ├── Acme.P21.Common/          # Shared utilities and services
│   └── Acme.P21.Data/            # Data access and repository layer
├── Acme.P21.Rules/               # Business rule implementations
├── Acme.P21.Tests/               # Unit tests with XML test data
├── TestApp/                      # WinForms testing application
└── TestConsole/                  # Console testing application
```

### Key Components

#### Business Rules (`Acme.P21.Rules`)
- **CallApiRule**: API integration for pricing requests
- **HeaderSourceLocChange**: Geographic-based warehouse assignment
- **NoInventorySourceChange**: Alternative inventory sourcing

#### Common Library (`Acme.P21.Common`)
- **Configuration Management**: Encrypted settings and connection strings
- **Logging Service**: Multi-destination structured logging
- **API Utilities**: REST client with JWT authentication
- **Encryption Services**: Security for sensitive configuration

#### Data Layer (`Acme.P21.Data`)
- **Repository Pattern**: Clean data access abstraction
- **Inventory Repository**: Prophet 21 database integration
- **Geographic Models**: Location and distance calculations

## Business Rules

### Geographic Warehouse Assignment
```
WHEN: Order state ≠ Ship-to state
THEN: Change source warehouse to ship-to state's primary location
PURPOSE: Optimize shipping efficiency and reduce costs
```

### Intelligent Inventory Sourcing
```
WHEN: Insufficient inventory at current warehouse
THEN: 
  1. Calculate distances to all company warehouses
  2. Check inventory levels at each location
  3. Select closest warehouse with sufficient stock
  4. Fall back to closest stockable warehouse if needed
  5. Mark as special order if no inventory available
PURPOSE: Maximize fulfillment while minimizing shipping distance
```

### Real-time Pricing Integration
```
WHEN: Pricing information required
THEN: Query Prophet 21 REST API for current pricing
PURPOSE: Ensure accurate, up-to-date pricing
```

## Technology Stack

- **.NET Framework 4.8.1**
- **Dapper ORM** for database access
- **RestSharp** for HTTP client operations
- **Serilog** for structured logging
- **MSTest** for unit testing
- **JWT** for API authentication

## External Integrations

- **Prophet 21 Database**: Direct SQL queries for inventory and location data
- **Prophet 21 REST API**: Real-time pricing and data synchronization
- **Logging Infrastructure**: Seq, Elasticsearch, SQL Server
- **Geographic Services**: ZIP code-based distance calculations

## Configuration

The system requires configuration for:

- **Database Connections**: Encrypted Prophet 21 database connection strings
- **API Credentials**: Consumer keys and authentication for Prophet 21 API
- **Logging Endpoints**: URLs for Seq, Elasticsearch, and other services
- **Business Parameters**: Company codes, warehouse mappings, geographic rules

## Getting Started

### Prerequisites
- .NET Framework 4.8.1
- Access to Prophet 21 database
- Prophet 21 API credentials

### Building
```bash
# Restore packages and build solution
msbuild Acme.P21.Rules.sln /p:Configuration=Release
```

### Testing
```bash
# Run unit tests
mstest /testcontainer:Acme.P21.Tests.dll
```

### Docker Development Environment
For development and testing with logging infrastructure:

```bash
# Start Seq and ELK stack
docker-compose up -d

# Access services
# Seq: http://localhost:5341
# Kibana: http://localhost:5601
# Elasticsearch: http://localhost:9200
```

## Business Value

### Operational Efficiency
- **Reduced Manual Intervention**: Automated warehouse selection decisions
- **Optimized Shipping Costs**: Distance-based fulfillment routing
- **Improved Order Fulfillment**: Alternative inventory sourcing

### Customer Service
- **Faster Order Processing**: Automated business rule execution
- **Higher Fill Rates**: Intelligent inventory allocation
- **Accurate Pricing**: Real-time API integration

### Visibility and Control
- **Comprehensive Audit Trails**: Complete logging of all decisions
- **Performance Monitoring**: Integration with modern logging platforms
- **Rule Transparency**: Clear business logic documentation

## Contributing

This is a custom extension for Prophet 21 ERP systems. Modifications should follow the established patterns and include appropriate unit tests with XML test data.

## License

Proprietary - Internal use for Prophet 21 distribution system extensions.