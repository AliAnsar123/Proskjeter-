package org.example.io;

import javafx.collections.ObservableList;
import org.example.Configuration;
import org.example.Product;

import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.nio.charset.StandardCharsets;
import java.nio.file.Path;

public class FileSaverCSV {
    public static void save(ObservableList<Product> products, Path path) throws IOException {
        try (OutputStreamWriter outputStreamWriter = new OutputStreamWriter(new FileOutputStream(String.valueOf(path)), StandardCharsets.ISO_8859_1)) {
            StringBuilder stringBuilder = new StringBuilder();

            products.forEach(product -> {
                stringBuilder.append(product.toString());
                stringBuilder.append(System.lineSeparator());
            });

            outputStreamWriter.write(stringBuilder.toString());
        } catch (IOException e) {
            throw new IOException("Kunne ikke skrive fil til sti: " + path.toString() + " : " + e.getMessage());
        }
    }
}