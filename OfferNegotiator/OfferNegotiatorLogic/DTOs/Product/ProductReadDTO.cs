﻿using OfferNegotiatorDal.Models.Enums;

namespace OfferNegotiatorLogic.DTOs.Product;

public record ProductReadDTO
(
    Guid Id,
    string Name,
    decimal Price,
    ProductState State
);