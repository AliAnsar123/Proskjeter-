package org.example.exceptions;

public class InvalidCategoryException extends IllegalArgumentException {
    public InvalidCategoryException(String string) {
        super("Produktkategori med navn '" + string + "' eksiterer allerede i registeret, velg et annet navn");
    }
}
