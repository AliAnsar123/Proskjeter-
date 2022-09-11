package org.example;

import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import org.example.exceptions.InvalidCategoryOptionException;
import org.example.exceptions.InvalidNameException;
import org.example.exceptions.InvalidCategoryException;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;
import java.util.ArrayList;

public class Category implements Serializable {
    private static final long serialVersionUID = 1;

    private transient SimpleStringProperty name;
    private transient ObservableList<CategoryOption> categoryOptions = FXCollections.observableArrayList();

    public Category(String categoryName, CategoryOption... CategoryOption) {
        setName(categoryName);
        this.categoryOptions.addAll(CategoryOption);
    }

    public ObservableList<CategoryOption> getOptions() {
        return categoryOptions;
    }

    public void addCategoryOption(CategoryOption newCategoryOption) {
        if (categoryOptions.stream().anyMatch(categoryOption -> newCategoryOption.getName().equals(categoryOption.getName())))
            throw new InvalidCategoryOptionException(newCategoryOption.getName());

        categoryOptions.add(newCategoryOption);
    }

    public String getName() {
        return name.get();
    }

    public void setName(String name) {
        // hvis komponenten allerede finnes vil den ikke blir opprettet
        PrimaryController.categoryRegister.forEach(category -> {
            if (category.getName().equals(name))
                throw new InvalidCategoryException(name);
        });
        if (!InputValidator.categoryOptionName(name))
            throw new InvalidNameException(name);

        this.name = new SimpleStringProperty(name);
    }

    private void writeObject(ObjectOutputStream objectOutputStream) throws IOException {
        objectOutputStream.writeUTF(name.get());
        objectOutputStream.writeObject(new ArrayList<>(categoryOptions));
    }

    private void readObject(ObjectInputStream objectInputStream) throws IOException, ClassNotFoundException {
        String categoryName = objectInputStream.readUTF();
        setName(categoryName);

        ArrayList<CategoryOption> CategoryOption = (ArrayList<CategoryOption>) objectInputStream.readObject();
        this.categoryOptions = FXCollections.observableArrayList(CategoryOption);
    }

    @Override
    public String toString() {
        return getName();
    }
}
