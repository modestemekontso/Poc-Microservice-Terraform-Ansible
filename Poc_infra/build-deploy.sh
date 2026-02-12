#!/bin/bash
# Build and Deploy Script for Microservices
# Usage: ./build-deploy.sh [command]

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Functions
print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_error() {
    echo -e "${RED}✗ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠ $1${NC}"
}

print_info() {
    echo -e "${YELLOW}ℹ $1${NC}"
}

# Check if .env exists
check_env() {
    if [ ! -f .env ]; then
        print_warning ".env file not found. Creating from .env.example..."
        cp .env.example .env
        print_info "Please update .env with your configuration before proceeding"
        exit 1
    fi
}

# Build all services
build_all() {
    print_info "Building all services..."
    docker-compose build
    print_success "All services built successfully"
}

# Build specific service
build_service() {
    if [ -z "$1" ]; then
        print_error "Please specify a service name (customer, order, or catalog)"
        exit 1
    fi
    
    SERVICE="${1}-service"
    print_info "Building ${SERVICE}..."
    docker-compose build ${SERVICE}
    print_success "${SERVICE} built successfully"
}

# Start all services
start_all() {
    print_info "Starting all services..."
    docker-compose up -d
    print_success "All services started"
    
    print_info "Waiting for services to be healthy..."
    sleep 10
    
    check_health
}

# Stop all services
stop_all() {
    print_info "Stopping all services..."
    docker-compose down
    print_success "All services stopped"
}

# Restart all services
restart_all() {
    print_info "Restarting all services..."
    docker-compose restart
    print_success "All services restarted"
}

# View logs
view_logs() {
    if [ -z "$1" ]; then
        docker-compose logs -f
    else
        SERVICE="${1}-service"
        docker-compose logs -f ${SERVICE}
    fi
}

# Check health of all services
check_health() {
    print_info "Checking service health..."
    
    # Check databases
    check_service_health "customerdb" "5432"
    check_service_health "orderdb" "5432"
    check_service_health "catalogdb" "5432"
    
    # Check RabbitMQ
    check_service_health "rabbitmq" "5672"
    
    # Check microservices
    check_http_health "customer-service" "http://localhost:5001/health"
    check_http_health "order-service" "http://localhost:5002/health"
    check_http_health "catalog-service" "http://localhost:5003/health"
}

check_service_health() {
    SERVICE=$1
    PORT=$2
    
    if docker-compose ps | grep -q "${SERVICE}.*Up"; then
        print_success "${SERVICE} is running"
    else
        print_error "${SERVICE} is not running"
    fi
}

check_http_health() {
    SERVICE=$1
    URL=$2
    
    if curl -f -s ${URL} > /dev/null 2>&1; then
        print_success "${SERVICE} is healthy"
    else
        print_warning "${SERVICE} health check failed or not ready yet"
    fi
}

# Clean everything
clean_all() {
    print_warning "This will remove all containers, networks, and volumes!"
    read -p "Are you sure? (y/N) " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        print_info "Cleaning up..."
        docker-compose down -v
        print_success "Cleanup complete"
    else
        print_info "Cleanup cancelled"
    fi
}

# Run tests
run_tests() {
    print_info "Running tests..."
    
    # Test CustomerService
    if [ -d "CustomerService" ]; then
        print_info "Testing CustomerService..."
        dotnet test CustomerService/CustomerService.csproj
    fi
    
    # Test OrderService
    if [ -d "OrderService" ]; then
        print_info "Testing OrderService..."
        dotnet test OrderService/OrderService.csproj
    fi
    
    # Test CatalogService
    if [ -d "CatalogService" ]; then
        print_info "Testing CatalogService..."
        dotnet test CatalogService/CatalogService.csproj
    fi
    
    print_success "All tests completed"
}

# Database backup
backup_databases() {
    BACKUP_DIR="./backups/$(date +%Y%m%d_%H%M%S)"
    mkdir -p ${BACKUP_DIR}
    
    print_info "Backing up databases to ${BACKUP_DIR}..."
    
    docker-compose exec -T customerdb pg_dump -U postgres customerdb > ${BACKUP_DIR}/customerdb.sql
    docker-compose exec -T orderdb pg_dump -U postgres orderdb > ${BACKUP_DIR}/orderdb.sql
    docker-compose exec -T catalogdb pg_dump -U postgres catalogdb > ${BACKUP_DIR}/catalogdb.sql
    
    print_success "Database backups completed"
}

# Show usage
usage() {
    cat << EOF
Microservices Build and Deploy Script

Usage: $0 [command] [options]

Commands:
    build [service]     Build all services or specific service (customer, order, catalog)
    start               Start all services
    stop                Stop all services
    restart             Restart all services
    logs [service]      View logs (all or specific service)
    health              Check health of all services
    test                Run tests for all services
    backup              Backup all databases
    clean               Clean all containers, networks, and volumes
    help                Show this help message

Examples:
    $0 build                    # Build all services
    $0 build customer           # Build customer service only
    $0 start                    # Start all services
    $0 logs                     # View all logs
    $0 logs order               # View order service logs
    $0 health                   # Check health status
    $0 backup                   # Backup databases

EOF
}

# Main script
main() {
    case "$1" in
        build)
            check_env
            if [ -z "$2" ]; then
                build_all
            else
                build_service "$2"
            fi
            ;;
        start)
            check_env
            start_all
            ;;
        stop)
            stop_all
            ;;
        restart)
            restart_all
            ;;
        logs)
            view_logs "$2"
            ;;
        health)
            check_health
            ;;
        test)
            run_tests
            ;;
        backup)
            backup_databases
            ;;
        clean)
            clean_all
            ;;
        help|--help|-h)
            usage
            ;;
        *)
            print_error "Unknown command: $1"
            usage
            exit 1
            ;;
    esac
}

# Run main function with all arguments
main "$@"