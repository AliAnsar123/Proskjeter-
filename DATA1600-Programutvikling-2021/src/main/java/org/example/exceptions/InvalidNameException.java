package org.example.exceptions;

public class InvalidNameException extends IllegalArgumentException {
    public InvalidNameException(String string) {
        super("Produktkategori-/egenskapsnavn '" + string + "' kan ikke inneholde ',' (komma) eller være tomt");
    }
}
