package org.example;

import javafx.beans.property.SimpleStringProperty;
import javafx.collections.ObservableList;
import org.example.exceptions.InvalidNameException;

import java.util.HashMap;
import java.util.Map;
import java.util.stream.Collector;
import java.util.stream.Collectors;

public class Product {
    private SimpleStringProperty name;
    private Category category;
    private Map<CategoryOption, String> categoryOptions = new HashMap<>();

    public Product(String name, Category category) {
        setName(name);
        setCategory(category);
    }

    public String getName() {
        return name.get();
    }

    public void setName(String name) {
        if (!InputValidator.productName(name))
            throw new InvalidNameException(name);

        this.name = new SimpleStringProperty(name);
    }

    public Category getCategory() {
        return category;
    }

    public void setCategory(Category category) {
        if (category == null)
            throw new NullPointerException();

        this.category = category;
    }

    public String getCategoryOption(CategoryOption categoryOption) {
        return categoryOptions.get(categoryOption);
    }

    public void setCategoryOptions(CategoryOption categoryOption, String name) {
        if (!InputValidator.categoryOptionName(name))
            throw new InvalidNameException(name);

        categoryOptions.put(categoryOption, name);
    }

    @Override
    public String toString() {
        StringBuilder categoryOptionsString = new StringBuilder();
        category.getOptions().forEach(categoryOption -> categoryOptionsString.append(categoryOption.toCSV()));

        return String.format(",%s,%s,%s", name.get(), category.getName(), categoryOptionsString);
    }
}