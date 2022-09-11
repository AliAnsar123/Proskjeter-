package org.example.io;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import org.example.Category;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.ArrayList;

public class FileOpenerJobj {
    public static ObservableList<Category> open(Path filePath) throws IOException {
        try (ObjectInputStream objectInputStream = new ObjectInputStream(Files.newInputStream(filePath))) {
            ArrayList<Category> object = (ArrayList<Category>) objectInputStream.readObject();
            return FXCollections.observableArrayList(object);
        } catch (IOException | ClassNotFoundException e) {
            System.out.println("e = " + e);
            throw new IOException("Kunne ikke åpne Jobj-fil: " + e.getMessage());
        }
    }
}