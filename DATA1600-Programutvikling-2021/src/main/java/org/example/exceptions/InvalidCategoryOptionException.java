package org.example.exceptions;

public class InvalidCategoryOptionException extends IllegalArgumentException {
    public InvalidCategoryOptionException(String string) {
        super("Egenskap med navn '" + string + "' eksiterer allerede i denne kategorien, velg et annet navn");
    }
}
