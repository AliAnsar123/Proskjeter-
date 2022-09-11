package org.example.io;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import org.example.CategoryOption;
import org.example.Configuration;

import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
import java.nio.file.Path;
import java.util.Arrays;

public class FileOpenerCSV {
    public static ObservableList<Configuration> open(Path filePath) throws IOException {
        try (BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(new FileInputStream(String.valueOf(filePath)), StandardCharsets.ISO_8859_1));) {
            ObservableList<Configuration> observableList = FXCollections.observableArrayList();

            String line;
            while ((line = bufferedReader.readLine()) != null) {
                observableList.add(readConfiguration(line));
            }

            return observableList;
        } catch (IOException e) {
            throw new IOException("Kunne ikke åpne CSV-fil: " + e.getMessage());
        }
    }

    private static Configuration readConfiguration(String line) throws IOException {
        String[] split = line.split(",");

        System.out.println(Arrays.toString(split));
        // TODO: fikse lesing fra CSV
        try {
            CategoryOption[] CategoryOptions = new CategoryOption[(split.length-2)/3];

            // looper gjennom alle komponentene i linjen, starter på index 2 fordi 0 og 1 er navn og e-post
            for (int i = 0, j=0; i < split.length; i+=3, j++) {
                String componentName = split[i];
                String optionName = split[i+1];

                //CategoryOptions[j] = new CategoryOption(componentName, optionName);
            }

            return new Configuration(CategoryOptions);
        } catch (NumberFormatException e) {
            throw new IllegalArgumentException("Kunne ikke opprette elementer fra brukerdata: komponentpris må oppgis i heltall");
        } catch (Exception e) {
            throw new IOException("Kunne ikke opprette elementer fra brukerdata: " + e.getMessage());
        }
    }
}