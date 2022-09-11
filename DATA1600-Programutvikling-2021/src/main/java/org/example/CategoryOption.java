package org.example;

import javafx.beans.property.SimpleStringProperty;
import org.example.exceptions.InvalidNameException;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;

public class CategoryOption implements Serializable {
    private static final long serialVersionUID = 1;

    private transient SimpleStringProperty name;

    public CategoryOption(Category category, String name) {
        setName(name);
    }

    public String getName() {
        return name.get();
    }

    public void setName(String name) {
        if (!InputValidator.categoryOptionName(name))
            throw new InvalidNameException(name);

        this.name = new SimpleStringProperty(name);
    }

    private void writeObject(ObjectOutputStream objectOutputStream) throws IOException {
        objectOutputStream.writeUTF(getName());
    }

    private void readObject(ObjectInputStream objectInputStream) throws IOException, ClassNotFoundException {
        setName(objectInputStream.readUTF());
    }

    @Override
    public String toString() {
        return getName();
    }

    public String toCSV() {
        return String.format(",%s",/*getCategory().getName(),*/ getName());
    }
}
