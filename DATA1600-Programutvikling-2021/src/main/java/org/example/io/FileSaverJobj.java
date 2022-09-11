package org.example.io;

import javafx.collections.ObservableList;
import org.example.Category;

import java.io.IOException;
import java.io.ObjectOutputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.ArrayList;

public class FileSaverJobj {
    public static void save(ObservableList<Category> objects, Path filePath) throws IOException {
        try (ObjectOutputStream objectOutputStream = new ObjectOutputStream(Files.newOutputStream(filePath))) {
            ArrayList<Category> objectList = new ArrayList<>(objects);
            objectOutputStream.writeObject(objectList);
        } catch (Exception e) {
            throw new IOException("Kunne ikke skrive fil til sti: " + filePath.toString() + " : " + e.getMessage());
        }
    }
}