package org.example;

import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;

public class Configuration {
    private SimpleStringProperty name, email;
    private ObservableList<CategoryOption> CategoryOptions = FXCollections.observableArrayList();

    public Configuration(CategoryOption... componentOptions) {
        this.CategoryOptions.addAll(componentOptions);
    }

    public ObservableList<CategoryOption> getCategoryOptions() {
        return CategoryOptions;
    }

    // brukes til å lagre til CSV
    @Override
    public String toString() {
        StringBuilder CategoryOptionsString = new StringBuilder();
        CategoryOptions.forEach(CategoryOption -> CategoryOptionsString.append(CategoryOption.toCSV()));

        return String.format("%s,%s%s", name.get(), email.get(), CategoryOptionsString);
    }
}
