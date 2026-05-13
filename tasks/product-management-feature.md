# product-management-feature.md

# Product Management Feature Specification

You are a senior .NET software architect and UI/UX designer.

I am developing an Admin Panel for a corporate plastic factory website using ASP.NET Core (.NET 10), Clean Architecture, SQL Server, and AdminLTE UI.

This website is NOT an e-commerce platform. Products are displayed only as industrial catalog items. Users cannot purchase products online.

I need you to design and implement a professional and scalable Product Management feature for the admin panel.

---

# Core Product Requirements

Each product should contain the following fields:

## General Information

- Title
- Slug (SEO Friendly URL)
- Short Description
- Full Description (Rich Text / HTML Editor)
- Main Image
- Product Category
- Product Tags
- Is Featured
- Publish Status
- Sort Order

---

# SEO Fields

- SEO Meta Title
- SEO Meta Description
- SEO Keywords
- Canonical URL (Optional)

---

# Media & Files

- Product Gallery Images
- Catalog PDF File
- Video URL
- Thumbnail Image

---

# Audit Information

- CreatedAt
- UpdatedAt
- CreatedBy
- UpdatedBy

---

# Dynamic Product Attributes

IMPORTANT:

Do NOT create fixed industrial database columns such as:

- Material
- Weight
- Dimensions
- Thickness
- Capacity
- Color

Instead, implement a flexible and dynamic ProductAttribute system using Key-Value pairs.

## ProductAttribute Structure

- Id
- ProductId
- Key
- Value
- SortOrder

## Example Data

| Key | Value |
|---|---|
| Material | Polyethylene |
| Weight | 120g |
| Thickness | 0.8mm |
| Color | Black |
| Capacity | 500ml |

---

# Product Attribute Requirements

Admin users must be able to:

- Add unlimited attributes
- Edit attributes dynamically
- Delete attributes
- Reorder attributes using drag & drop
- Group attributes visually in UI
- Save attributes asynchronously if possible

---

# Product Categories

Products belong to categories.

## Category Fields

- Title
- Slug
- Description
- ParentCategoryId
- Image/Icon
- SortOrder
- IsPublished

## Category SEO

- Meta Title
- Meta Description

---

# Product Gallery

Each product can contain multiple gallery images.

## Gallery Requirements

Admin panel must support:

- Multiple image upload
- Image preview
- Drag & drop sorting
- Remove image
- Set primary image
- Lazy loading support
- Optimized image storage structure

---

# Product Inquiry Feature

Because the website is not an online shop, products should support inquiry requests instead of purchasing.

Prepare backend structure for:

## ProductInquiry

- Id
- ProductId
- CustomerName
- PhoneNumber
- CompanyName
- Email
- Description
- CreatedAt
- IsReviewed

---

# Admin Panel UI Requirements

The admin UI should be modern, clean, and optimized for usability using AdminLTE.

## Suggested UI Sections

1. General Information
2. SEO
3. Gallery
4. Dynamic Attributes
5. Files & Media
6. Inquiry Settings

---

# UI/UX Expectations

Use:

- Tabs or Accordion Sections
- Repeater UI for attributes
- Rich Text Editor
- Image Upload Preview
- Responsive Layout
- Validation Messages
- AJAX-based interactions where appropriate
- Drag & Drop support

The interface should feel modern and enterprise-grade.

---

# Technical Requirements

Use the following stack:

- ASP.NET Core (.NET 10)
- Clean Architecture
- CQRS Pattern
- MediatR
- Entity Framework Core
- FluentValidation
- Repository Pattern
- SQL Server

---

# Generate the Following

Please generate:

- Database Design
- Entity Models
- EF Core Configurations
- DTOs
- Commands & Queries
- Validators
- Services
- Repository Interfaces
- Admin MVC/Razor Views
- API Endpoints if necessary
- Folder Structure
- Dependency Injection Setup

---

# Additional Requirements

Focus heavily on:

- Scalability
- Maintainability
- Extensibility
- SEO Readiness
- Performance
- Clean Code Principles
- Enterprise Architecture
- Industrial Catalog Structure

The final implementation should be production-ready and suitable for long-term enterprise development.
