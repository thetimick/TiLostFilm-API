namespace TiLostFilm.Entities.Error;

public record ErrorEntity(
    int Status, 
    string ErrorMessage
);