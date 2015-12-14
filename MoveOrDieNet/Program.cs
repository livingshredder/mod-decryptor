﻿using System;
using System.IO;
using System.Linq;

namespace MoveOrDieNet
{
    class Program
    {
        static byte[] XORTable = new byte[] {
            0xDF, 0xEC, 0x80, 0xEA, 0xBC, 0xFD, 0xD4, 0x24, 0xEF, 0x4D,
            0x5A, 0xAA, 0x25, 0x5D, 0x19, 0x1E, 0x27, 0x8B, 0x7F, 0xEB,
            0x31, 0x00, 0xCA, 0x0F, 0x27, 0x48, 0x80, 0x06, 0x66, 0x37,
            0x6B, 0xDC, 0xD6, 0x8E, 0x8D, 0x4D, 0xCA, 0x11, 0x07, 0x40,
            0x9E, 0x5E, 0xDF, 0xF0, 0x41, 0xC5, 0xF3, 0x1D, 0x20, 0x47,
            0xC4, 0x1E, 0x7F, 0x23, 0x94, 0x95, 0x1A, 0x97, 0x25, 0x72,
            0x23, 0x49, 0x75, 0x2C, 0x21, 0x36, 0x3A, 0x71, 0x72, 0x02,
            0xC8, 0x09, 0x41, 0x5F, 0x76, 0x23, 0x97, 0x80, 0x91, 0x32,
            0x19, 0x48, 0x39, 0x2F, 0xB1, 0x17, 0x4A, 0x3F, 0x48, 0xCD,
            0x8A, 0x0F, 0xD3, 0x19, 0x55, 0x4E, 0x42, 0x15, 0x14, 0x23,
            0x30, 0x6B, 0xA5, 0x6F, 0x31, 0x18, 0x67, 0xDB, 0xB2, 0xBC,
            0xBA, 0x4E, 0xCF, 0xC1, 0x52, 0x70, 0x4F, 0xB6, 0xDC, 0x16,
            0x0A, 0x51, 0x6E, 0xDB, 0x4F, 0x7B, 0xD5, 0xBD, 0xF2, 0x3C,
            0xB3, 0x52, 0xC0, 0x93, 0xC8, 0x7F, 0x5E, 0xF1, 0x0A, 0xDC,
            0x72, 0xB0, 0x8D, 0x1F, 0x4B, 0xFB, 0x10, 0x5F, 0x40, 0xCB,
            0x88, 0xA3, 0xC1, 0xE8, 0xB2, 0x81, 0x73, 0x7E, 0x74, 0x82,
            0x25, 0x6E, 0xC4, 0xD8, 0xB6, 0x97, 0x73, 0x7F, 0x82, 0xCC,
            0x98, 0x54, 0xF7, 0x9F, 0xD9, 0x28, 0x2E, 0x91, 0x20, 0x82,
            0xF5, 0xEB, 0x10, 0x1C, 0x5E, 0x08, 0xAE, 0xDA, 0xF1, 0xCF,
            0xC9, 0xD8, 0x32, 0x7E, 0x20, 0x02, 0x1B, 0xA9, 0x7E, 0x8E,
            0x62, 0x5C, 0x62, 0xEE, 0xC8, 0x51, 0x78, 0x15, 0xA8, 0x11,
            0x8F, 0x6B, 0x25, 0x74, 0xCF, 0xB4, 0xD1, 0xBE, 0x07, 0x1E,
            0x71, 0x87, 0x85, 0xB1, 0x79, 0xA8, 0x8D, 0x1E, 0x14, 0x75,
            0xFD, 0x43, 0x99, 0xFE, 0xE7, 0xE0, 0xB1, 0x95, 0x57, 0x64,
            0x4A, 0xC0, 0x5B, 0x4A, 0x5F, 0x0D, 0xC3, 0x50, 0x3A, 0x26,
            0x45, 0x08, 0xFB, 0xC2, 0xBB, 0x00, 0xAD, 0x9B, 0x96, 0x46,
            0xAD, 0x85, 0x96, 0xB2, 0x28, 0x55, 0x0C, 0x84, 0x29, 0x99,
            0x2B, 0xB9, 0x57, 0x5B, 0xA2, 0xFC, 0xDC, 0x4A, 0x78, 0xF4,
            0x5A, 0xC3, 0x61, 0x09, 0x6E, 0x45, 0x1F, 0x28, 0xC6, 0x2F,
            0x6E, 0x6E, 0x44, 0x96, 0xEF, 0x1C, 0x58, 0xB2, 0x0E, 0x1A,
            0x8F, 0x26, 0xFB, 0xEC, 0xFE, 0x0D, 0x25, 0x2D, 0x02, 0x82,
            0x60, 0xB8, 0x7E, 0x1D, 0xDA, 0x50, 0x09, 0xE3, 0x70, 0xC1,
            0x60, 0xAB, 0x28, 0x4C, 0xD2, 0x18, 0xC5, 0xDE, 0x7E, 0x19,
            0xC3, 0xD5, 0x2D, 0xF3, 0xF9, 0x25, 0x78, 0x21, 0xC2, 0x50,
            0x39, 0xA4, 0xDA, 0xD5, 0x6A, 0x9B, 0x2E, 0xC4, 0xD7, 0x84,
            0x8C, 0x13, 0xF4, 0x95, 0xCF, 0xF3, 0x14, 0x94, 0xC7, 0x8E,
            0x0B, 0x51, 0x99, 0xC1, 0x9B, 0x2A, 0xEF, 0x73, 0x4C, 0x13,
            0x1E, 0x58, 0x90, 0x9F, 0x59, 0xDE, 0x41, 0xAE, 0x8D, 0x85,
            0xCB, 0x3D, 0xD4, 0xAE, 0x64, 0x55, 0x7A, 0x45, 0x97, 0xBF,
            0x53, 0xFF, 0x9F, 0x2F, 0x46, 0xF4, 0xAB, 0x1A, 0x67, 0xA3,
            0xD2, 0xAD, 0xFB, 0x47, 0x26, 0xB6, 0x2F, 0x5A, 0x35, 0x01,
            0xC7, 0xF1, 0x20, 0x54, 0xBA, 0x27, 0xE4, 0xB9, 0x62, 0x0F,
            0xE9, 0xF3, 0x6A, 0x2E, 0x48, 0x0F, 0xE1, 0xDA, 0x6D, 0x05,
            0xFF, 0xE2, 0xC3, 0x40, 0x1F, 0x12, 0xBC, 0xAF, 0xA6, 0xF2,
            0x10, 0x75, 0x91, 0x80, 0x5D, 0x62, 0x15, 0x30, 0xD6, 0x61,
            0xB0, 0xE1, 0xDC, 0x99, 0xAB, 0xEA, 0xBB, 0x48, 0xA3, 0x0C,
            0x2C, 0x31, 0x20, 0x06, 0xEF, 0x65, 0xE2, 0x2E, 0xCC, 0x20,
            0xAB, 0xCA, 0xFE, 0xE2, 0x41, 0xFC, 0x2B, 0xE5, 0xEA, 0x9F,
            0xEB, 0x15, 0xBE, 0x8E, 0x08, 0x6F, 0xCD, 0x04, 0x6C, 0xA5,
            0x4D, 0x05, 0x36, 0x85, 0x02, 0xD5, 0x2C, 0x2C, 0xA2, 0x47,
            0xA4, 0xBC, 0x93, 0x79, 0x06, 0x88, 0xBC, 0xC9, 0xDA, 0xA2,
            0x2D, 0x2E, 0x80, 0xCC, 0x21, 0xA3, 0x58, 0x3F, 0xA4, 0x6A,
            0x6A, 0x17, 0xA5, 0x7D, 0x70, 0x74, 0x4B, 0xD0, 0x08, 0x36,
            0x79, 0x8D, 0xCC, 0xFF, 0xE4, 0xBA, 0x45, 0x90, 0xFB, 0x20,
            0x21, 0xC2, 0xE3, 0xDD, 0x31, 0x0A, 0x84, 0x38, 0xC9, 0x5A,
            0x01, 0xFC, 0xD9, 0xCF, 0xA2, 0xEF, 0xB1, 0x69, 0xBF, 0x11,
            0x9B, 0x5B, 0xB7, 0x78, 0x27, 0x30, 0xA3, 0x1C, 0x75, 0x1E,
            0x02, 0x39, 0x26, 0x6F, 0xB4, 0x0E, 0xD8, 0xEB, 0x83, 0x26,
            0x35, 0xD4, 0xF2, 0x07, 0xC3, 0x39, 0x52, 0x62, 0x49, 0xFA,
            0x77, 0x07, 0xF8, 0x4A, 0x37, 0x27, 0x06, 0x2A, 0x57, 0x74,
            0x54, 0x61, 0x65, 0xCC, 0xA7, 0x56, 0x74, 0xB6, 0xBE, 0x8C,
            0x3D, 0x69, 0x00, 0x3F, 0x4C, 0x20, 0x26, 0xD6, 0xC9, 0x39,
            0x26, 0x32, 0x12, 0x8F, 0x3A, 0xC2, 0xC4, 0xEF, 0x75, 0x02,
            0x96, 0x9B, 0x8F, 0x94, 0xDA, 0xBD, 0x17, 0x87, 0x2B, 0x18,
            0x73, 0x2A, 0xDC, 0x63, 0x8C, 0x9B, 0x02, 0x0E, 0x31, 0x62,
            0x4E, 0x3B, 0xC6, 0xAC, 0x5E, 0xD6, 0x9F, 0x96, 0xAC, 0x39,
            0x4C, 0x2F, 0x60, 0x27, 0x9E, 0xFD, 0x9E, 0xFF, 0xAE, 0x2C,
            0x91, 0x5B, 0x6F, 0x90, 0x57, 0x6F, 0x79, 0x2B, 0x01, 0xE8,
            0x24, 0x49, 0x5C, 0x09, 0x88, 0x4A, 0x5D, 0x7C, 0xD2, 0x01,
            0xC8, 0x35, 0x47, 0x6B, 0xD4, 0xE2, 0xFD, 0xDF, 0xDB, 0xBE,
            0xB4, 0xF3, 0x68, 0xDA, 0xE4, 0x2B, 0x9C, 0x4C, 0x3C, 0x31,
            0xBA, 0x6F, 0x72, 0x9F, 0x06, 0x51, 0x8C, 0xC4, 0x26, 0x95,
            0x77, 0x8F, 0x17, 0xBF, 0x13, 0x81, 0xAE, 0x06, 0x14, 0x70,
            0x12, 0xA5, 0xCD, 0xA9, 0x5B, 0xAC, 0xC8, 0x3E, 0xCD, 0x0D,
            0x0D, 0x45, 0x99, 0x43, 0xAA, 0xD5, 0x81, 0x86, 0x86, 0x7F,
            0x47, 0x1D, 0x7A, 0x7D, 0xE0, 0x27, 0x27, 0x65, 0x49, 0xA1,
            0x7A, 0xB3, 0x62, 0xD2, 0x98, 0xCC, 0xEB, 0x76, 0x66, 0xDA,
            0x1E, 0x5A, 0x10, 0x54, 0xED, 0xEF, 0x68, 0x3E, 0xB1, 0xA0,
            0x17, 0xFE, 0x01, 0xEE, 0x4C, 0xBB, 0xD5, 0x72, 0x67, 0xD6,
            0x03, 0xFD, 0xB2, 0xF8, 0x0C, 0xF5, 0x7E, 0xA9, 0x49, 0x8B,
            0x7F, 0x70, 0x81, 0x36, 0xE4, 0x70, 0x72, 0x64, 0x34, 0x4C,
            0xDD, 0x65, 0x4D, 0x8D, 0x4E, 0xAE, 0x4C, 0x8C, 0xC2, 0xD9,
            0x27, 0x5D, 0x9E, 0x58, 0xAA, 0x66, 0x09, 0xC3, 0x0D, 0xA1,
            0xCC, 0xE1, 0xB1, 0xBE, 0x58, 0x27, 0x7E, 0xF9, 0xAA, 0xB4,
            0xBC, 0x17, 0xF8, 0x14, 0x65, 0x82, 0x39, 0x3D, 0xAF, 0x08,
            0x34, 0x07, 0xF7, 0x7E, 0x8F, 0x59, 0x5C, 0x4B, 0x58, 0xDB,
            0x9F, 0x90, 0x9B, 0xF2, 0xB2, 0x74, 0x7C, 0x0B, 0x40, 0x06,
            0xF8, 0x55, 0x2A, 0xE8, 0x56, 0xC2, 0xC8, 0x87, 0xCD, 0xE4,
            0xA9, 0x4B, 0x42, 0xA0, 0xF2, 0x3C, 0x4D, 0x21, 0x9D, 0x8B,
            0x7E, 0x2D, 0x3B, 0xBC, 0xE4, 0x75, 0x02, 0x39, 0x09, 0x3E,
            0xE9, 0x4E, 0x9D, 0xE9, 0x13, 0xA9, 0x9E, 0xF9, 0x16, 0x67,
            0x7C, 0x92, 0x98, 0x08, 0x5F, 0x99, 0xF3, 0xCA, 0xE5, 0xE4,
            0x73, 0x7C, 0xB9, 0xE7, 0x3F, 0xCE, 0x02, 0x80, 0xC9, 0x85,
            0x75, 0x71, 0xC6, 0xF7, 0x56, 0xFA, 0xCC, 0xA4, 0x55, 0x05,
            0x69, 0x73, 0xBB, 0x3E, 0x24, 0xDB, 0x32, 0xDE, 0x86, 0xCD,
            0xFA, 0x10, 0xCB, 0x76, 0x75, 0x08, 0xF2, 0x3B, 0x3A, 0x21,
            0xE8, 0xAE, 0xD6, 0xA0, 0x2D, 0x9E, 0x39, 0xFC, 0x71, 0x4F,
            0x4E, 0x7E, 0x36, 0xB5, 0x43, 0x4B, 0x5E, 0x00, 0xFD, 0x3C,
            0x0C, 0x6B, 0x30, 0xB0, 0x18, 0x96, 0x3D, 0x83, 0x45, 0xD5,
            0xBA, 0xB3, 0x1D, 0x50, 0xE3, 0x83, 0x92, 0x57, 0x32, 0x36,
            0x82, 0xF0, 0x15, 0x33, 0xA5, 0x34, 0x00, 0xEA, 0x25, 0x3C,
            0x58, 0x46, 0x0A, 0x84, 0xBD, 0x82, 0x07, 0x82, 0x0C, 0xBC,
            0xB0, 0x30, 0xEE, 0xF5, 0x98, 0x5A, 0xF4, 0xEA, 0x75, 0x29,
            0xE8, 0x2E, 0xA6, 0x8D, 0x0F, 0xEB, 0x8D, 0xC4, 0xBC, 0x19,
            0xDA, 0xCF, 0x04, 0x22, 0x6F, 0xC9, 0x12, 0x9E, 0x48, 0x5C,
            0x97, 0x99, 0x37, 0x52, 0xA1, 0x81, 0x75, 0xB0, 0x89, 0xF4,
            0xA8, 0x8D, 0xAB, 0x97, 0x5A, 0x00, 0xA8, 0x76, 0x2F, 0xF6,
            0x52, 0x8E, 0xF5, 0x88, 0xCF, 0xDA, 0xBA, 0x3E, 0x4A, 0x00,
            0x9C, 0x1F, 0xDC, 0x67, 0x8A, 0x47, 0xF0, 0xB5, 0xDE, 0x12,
            0x3F, 0x86, 0xF0, 0x0E, 0x32, 0x42, 0x38, 0x61, 0x89, 0x71,
            0x63, 0xEC, 0x41, 0xB7, 0x9A, 0x1D, 0x6D, 0x3F, 0x74, 0x5D,
            0x22, 0xAB, 0x66, 0x07, 0x95, 0x99, 0xF1, 0x33, 0xE0, 0x1C,
            0xB8, 0xFF, 0x4B, 0x19, 0x06, 0x92, 0xD5, 0xFE, 0x9B, 0xF7,
            0x94, 0x3A, 0xB1, 0xFC, 0x32, 0x89, 0xF0, 0xB3, 0xFB, 0x97,
            0x17, 0x26, 0x61, 0xE9, 0x40, 0x0C, 0xFE, 0x07, 0x3E, 0x41,
            0x4C, 0x05, 0x2C, 0x4D, 0x51, 0x23, 0x09, 0xCB, 0x5A, 0xCB,
            0xF3, 0xE8, 0xAA, 0x1B, 0x76, 0x84, 0xC2, 0x73, 0x2C, 0x58,
            0x05, 0x17, 0xB0, 0x05, 0x30, 0xF9, 0xF4, 0x31, 0xE9, 0x22,
            0xD4, 0x6E, 0x00, 0xE1, 0x6B, 0xA5, 0x5A, 0x09, 0x75, 0x41,
            0xF8, 0xDF, 0x9F, 0xE1, 0xF0, 0x86, 0x25, 0x33, 0x00, 0x80,
            0x6D, 0x31, 0x02, 0xB7, 0xB9, 0x1A, 0x5D, 0x5F, 0x41, 0xBE,
            0x45, 0x52, 0xB7, 0x76, 0x69, 0x7E, 0x7D, 0x4D, 0x2B, 0x8E,
            0x5D, 0x23, 0x7C, 0x4B, 0xE3, 0x01, 0x28, 0x56, 0x74, 0x62,
            0x93, 0x2D, 0x57, 0xEC, 0xF4, 0x72, 0x90, 0xE8, 0x2C, 0xCC,
            0x2F, 0xD5, 0xD9, 0x1D, 0xBA, 0xB5, 0x74, 0xFB, 0x00, 0xBE,
            0x83, 0xD2, 0xAB, 0x08, 0x58, 0x56, 0x9F, 0xE6, 0x33, 0x58,
            0x8B, 0x3E, 0x93, 0xA8, 0x98, 0x05, 0x3D, 0x9E, 0xC8, 0x85,
            0x9F, 0x04, 0xA5, 0xCC, 0x7A, 0xA8, 0x75, 0x10, 0xF7, 0x8D,
            0x9F, 0x20, 0xF5, 0xF1, 0x58, 0xF0, 0x56, 0x4C, 0xFD, 0x43,
            0x7E, 0x79, 0x14, 0x73, 0xF4, 0x03, 0xED, 0xE5, 0xD7, 0x4E,
            0x70, 0x3F, 0xB8, 0xE9, 0xBD, 0x49, 0xD5, 0x04, 0x54, 0xF1,
            0x13, 0xD3, 0xEF, 0x4E, 0xF0, 0x65, 0x4B, 0xA1, 0x94, 0xAA,
            0x5F, 0xC0, 0x4C, 0x15, 0x0F, 0xCB, 0x8B, 0xE1, 0x16, 0xD9,
            0x8D, 0x22, 0x00, 0x18, 0x59, 0x8B, 0x4A, 0x3B, 0x8F, 0x19,
            0xE4, 0x0A, 0x5C, 0xAF, 0x1D, 0xD3, 0x09, 0x13, 0x9F, 0x2E,
            0xC7, 0xFD, 0x4E, 0x38, 0x61, 0x73, 0xA3, 0xC8, 0xE3, 0xA7,
            0x27, 0xA5, 0x55, 0x30, 0x02, 0x79, 0xAF, 0x8E, 0xCB, 0x94,
            0x90, 0xFE, 0x4E, 0xB4, 0x32, 0x42, 0x52, 0x11, 0x91, 0x8C,
            0xBE, 0x3F, 0x5D, 0x23, 0xCC, 0xD8, 0x47, 0x90, 0xC9, 0xA1,
            0x98, 0xE4, 0x2E, 0x07, 0xB3, 0x42, 0xB2, 0x08, 0xEF, 0x71,
            0x01, 0x30, 0xE1, 0x97, 0x39, 0xEC, 0x05, 0x17, 0x59, 0xB8,
            0x85, 0x2F, 0x8C, 0x8E, 0x8F, 0x2F, 0xB4, 0xD7, 0x37, 0x48,
            0x8D, 0xBC, 0x02, 0x0A, 0x1F, 0x20, 0x7C, 0x7F, 0x32, 0xD9,
            0xBC, 0x2D, 0x21, 0x29, 0xC8, 0xE7, 0xAA, 0x7A, 0x16, 0x6E,
            0x77, 0x63, 0xA5, 0xD7, 0x40, 0x5C, 0x4B, 0xA7, 0x10, 0x84,
            0x28, 0x25, 0x34, 0x03, 0xD5, 0x09, 0xC4, 0x26, 0x00, 0xC7,
            0x8C, 0x58, 0xDE, 0x25, 0xE6, 0xE7, 0x75, 0x08, 0x4E, 0xD0,
            0xD4, 0x5F, 0x83, 0xEF, 0xE7, 0x28, 0x22, 0xE8, 0x4F, 0x21,
            0x74, 0x8D, 0xEF, 0xCB, 0xD0, 0xDD, 0x65, 0x80, 0x54, 0xEA,
            0xCF, 0x9C, 0x03, 0x19, 0xB4, 0x36, 0x48, 0x36, 0xA1, 0x80,
            0xF4, 0x38, 0xA4, 0x87, 0xDD, 0xCF, 0x0D, 0x67, 0xB3, 0x0A,
            0xCD, 0x49, 0x58, 0xA0, 0xF8, 0x10, 0xF5, 0x22, 0x8A, 0xDE,
            0x56, 0xC1, 0xF6, 0x25, 0x79, 0x1D, 0xD2, 0xEE, 0xDA, 0x07,
            0xF6, 0xEB, 0x4B, 0x4B, 0x87, 0xE4, 0x1B, 0x35, 0x9B, 0x08,
            0x41, 0xD6, 0x92, 0xAE, 0x6B, 0xC4, 0x7C, 0x8B, 0x46, 0xEC,
            0x42, 0xA5, 0xCE, 0x01, 0x2D, 0x25, 0x13, 0x50, 0x1C, 0x14,
            0x80, 0xA8, 0x35, 0x3B, 0x00, 0x41, 0x5F, 0x78, 0x83, 0xBF,
            0xB6, 0x7A, 0x16, 0x1E, 0x49, 0x72, 0xB6, 0xDB, 0x56, 0xC6,
            0x88, 0xA0, 0x49, 0xE6, 0xB4, 0x7A, 0xF1, 0xFC, 0x5A, 0xA5,
            0x5E, 0xE7, 0x85, 0xCD, 0x45, 0x87, 0xA4, 0x12, 0x02, 0xFE,
            0xB8, 0x86, 0x6E, 0xCC, 0xC1, 0xCD, 0x61, 0xB9, 0x8C, 0xF8,
            0x84, 0xCB, 0x6A, 0xD9, 0xA1, 0x6B, 0xA2, 0x2E, 0x0A, 0xE0,
            0x4D, 0x74, 0x2A, 0x3D, 0x18, 0xFC, 0x89, 0x61, 0xCA, 0x8A,
            0x76, 0xFF, 0xD4, 0xD3, 0x60, 0xCE, 0x52, 0x58, 0x74, 0x8B,
            0x6F, 0x6A, 0xAD, 0x98, 0x77, 0xD4, 0x8D, 0x05, 0x3D, 0x59,
            0x00, 0x4F, 0x54, 0xB8, 0xC5, 0x5E, 0xC6, 0xDD, 0x1B, 0xFB,
            0xD1, 0x18, 0x87, 0x39, 0x74, 0x07, 0xCB, 0x97, 0x40, 0x59,
            0x1E, 0xB0, 0x7F, 0x56, 0x41, 0xAF, 0x0C, 0x29, 0x4C, 0xFD,
            0xC3, 0x49, 0xE5, 0xAD, 0x40, 0xF2, 0x05, 0x54, 0x19, 0x0E,
            0xF5, 0x5F, 0x2A, 0x2F, 0xF0, 0x31, 0xD3, 0x8C, 0x34, 0x3A,
            0xAC, 0x6C, 0xE4, 0x78, 0x8C, 0x71, 0x12, 0x03, 0x5E, 0xD0,
            0xCA, 0x6D, 0xE9, 0xD3, 0xDD, 0xD2, 0x1C, 0x0F, 0xC6, 0x28,
            0x4F, 0xED, 0x6E, 0x47, 0xC1, 0xBA, 0xF9, 0x1A, 0x68, 0x9C,
            0xC5, 0xFE, 0xF7, 0x6C, 0xAA, 0x3F, 0xA2, 0x42, 0x44, 0xE3,
            0x3D, 0x43, 0x25, 0xBE, 0x21, 0x63, 0x44, 0x1D, 0x57, 0xD0,
            0x13, 0x56, 0x23, 0x61, 0x13, 0x2A, 0x73, 0x11, 0x70, 0x04,
            0x37, 0x65, 0x03, 0xE1, 0xB4, 0x7E, 0x2A, 0x96, 0x95, 0x94,
            0xAF, 0x14, 0xC9, 0x02, 0x3C, 0x06, 0x9F, 0xD4, 0x78, 0x1C,
            0x15, 0x1D, 0x8A, 0xA2, 0x82, 0x14, 0x56, 0x8E, 0x39, 0x20,
            0xC5, 0x17, 0xCF, 0x6C, 0x37, 0x58, 0xBC, 0x8F, 0xB6, 0xDA,
            0x32, 0xFF, 0x23, 0x5F, 0x02, 0x34, 0x2A, 0x38, 0x3C, 0xCF,
            0xF1, 0xF2, 0xD8, 0x85, 0xBE, 0x01, 0x74, 0x53, 0x17, 0xBD,
            0x2E, 0x07, 0x05, 0x96, 0xEE, 0xF8, 0x94, 0x6C, 0xCF, 0x26,
            0x49, 0x22, 0xBA, 0xB8, 0xBE, 0x55, 0x60, 0xD5, 0x27, 0x12,
            0xB0, 0x72, 0x0E, 0xA8, 0x01, 0x47, 0xF9, 0x30, 0x5F, 0xA1,
            0xC4, 0xE4, 0xEE, 0x91, 0x29, 0xAF, 0xDF, 0x54, 0xBD, 0xEA,
            0x06, 0x4A, 0x50, 0x7C, 0x42, 0x66, 0xD8, 0xBD, 0x1E, 0x28,
            0xD1, 0x11, 0x87, 0x78, 0xE8, 0x11, 0x25, 0xCE, 0xAC, 0x82,
            0x89, 0x1B, 0x71, 0xC1, 0xC4, 0xE9, 0x56, 0x1E, 0x1D, 0xD7,
            0x0D, 0x22, 0x23, 0x6B, 0x0D, 0x4F, 0x24, 0x65, 0x2D, 0x5E,
            0x70, 0x6F, 0x8D, 0x08, 0x47, 0xA1, 0xB2, 0xFD, 0x60, 0xE0,
            0xC1, 0x3F, 0x79, 0x6C, 0xDC, 0x4B, 0x47, 0x9E, 0xB0, 0x0C,
            0x6A, 0x89, 0x4E, 0x91, 0xFC, 0xBA, 0x1F, 0xAF, 0x4C, 0xB3,
            0x1E, 0x1D, 0x24, 0xF5, 0x8A, 0xB0, 0xDD, 0x3C, 0x39, 0xBC,
            0xC9, 0x31, 0xE1, 0x3D, 0x3D, 0x66, 0xF8, 0x11, 0xA4, 0x86,
            0x3B, 0x93, 0xCA, 0x48, 0xB0, 0x10, 0x0D, 0x13, 0x2C, 0xA6,
            0x09, 0xCB, 0xB0, 0x7C, 0xC4, 0x5F, 0xEC, 0x31, 0x6A, 0xB0,
            0x5F, 0xC7, 0xB0, 0x53, 0x55, 0x16, 0x93, 0x1D, 0xAA, 0x2D,
            0xB0, 0x32, 0x28, 0x12, 0xA6, 0xC7, 0x2E, 0x7F, 0xC3, 0xE3,
            0xD0, 0x42, 0x67, 0x49, 0x4D, 0xEB, 0xCB, 0xDB, 0x94, 0xF2,
            0x72, 0x9C, 0x1A, 0x27, 0x3A, 0x6E, 0xB9, 0x7A, 0xA5, 0xA9,
            0x25, 0x9D, 0xED, 0xCE, 0x9B, 0x6B, 0xFD, 0xDA, 0x3C, 0x16,
            0x4C, 0x09, 0xB9, 0xD8, 0x82, 0x2F, 0xBB, 0xD9, 0xD6, 0x53,
            0xFE, 0xFF, 0x51, 0xF0, 0xE6, 0x12, 0x25, 0xB8, 0x03, 0xEF,
            0x23, 0x34, 0x4F, 0x62, 0xC9, 0x8C, 0xB2, 0x89
        };

        static string[] warnExts = new string[] { ".ogg", ".csv", ".sfk", ".txt", ".otf" };

        static void ConvertFile(string path)
        {
            Console.WriteLine("Converting " + path);

            string extension = Path.GetExtension(path);

            if (warnExts.Contains(extension))
            {
                Console.WriteLine("WARNING: Converting " + extension + " is usually unnecessary and may cause Move Or Die to no longer work!");
                Console.WriteLine("WARNING: You can revert this process by running the tool again on this file.");
            }

            FileStream file = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
            byte[] data = new byte[file.Length];

            file.Read(data, 0, (int)file.Length);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= XORTable[i % 0x800];
            }

            switch (extension)
            {
                case ".lua":
                case ".el":
                    file.Close();
                    File.Delete(path);
                    File.WriteAllBytes(Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (extension == ".lua" ? ".el" : ".lua"), data);
                    break;
                default:
                    file.SetLength(0);
                    file.Write(data, 0, data.Length);
                    file.Close();
                    break;
            }
        }

        static void ConvertDirectory(string path)
        {
            Console.WriteLine("Scanning " + path);

            foreach (var file in Directory.EnumerateDirectories(path))
            {
                ConvertDirectory(file);
            }

            foreach (var directory in Directory.EnumerateFiles(path))
            {
                ConvertFile(directory);
            }
        }

        static void Main(string[] args)
        {
            foreach(var arg in args)
            {

                if(File.Exists(arg))
                {
                    ConvertFile(arg);
                }
                else if(Directory.Exists(arg))
                {
                    ConvertDirectory(arg);
                }
                else
                {
                    Console.WriteLine("File does not exist: " + arg);
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
