package org.example;

public class InputValidator {
    // TODO
    static boolean categoryName(String categoryName) {
        return categoryName.matches("[^,]+");
    }

    static boolean categoryOptionName(String categoryOptionName) {
        return categoryOptionName.matches("[^,]+");
    }

    static boolean productName(String productName) {
        return productName.matches("[^,]+");
    }
}
