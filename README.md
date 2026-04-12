
# Tetrahedron Sierpinski — Wizualizacja fraktala 3D (WPF)

![dotnet-8](https://img.shields.io/badge/.NET-8-brightgreen) ![license](https://img.shields.io/github/license/Znajka/M1)

## Co robi projekt

Aplikacja WPF (C#, .NET 8) renderuje fraktal Sierpińskiego zbudowany z tetraedrów w przestrzeni 3D. Umożliwia interaktywne przeglądanie fraktala, sterowanie poziomem rekurencji oraz automatyczną animację rotacji i przyrostu poziomów.

## Dlaczego to jest przydatne

- Wizualizacja fraktali 3D — przydatna do nauki grafiki 3D i algorytmów rekurencyjnych.
- Lekka demonstracja możliwości `Viewport3D` i `MeshGeometry3D` we WPF.
- Prosty punkt startowy do eksperymentów z wydajnością i renderowaniem proceduralnym.

## Najważniejsze funkcje

- Interaktywna rotacja modelu (przytrzymaj i przeciągnij lewym przyciskiem myszy).
- Przybliżanie/oddalanie kółkiem myszy.
- Sterowanie poziomem fraktala za pomocą suwaka (0–7, domyślnie 5).
- Automatyczne budowanie kolejnych poziomów i automatyczna rotacja (przyciski Start/Stop).
- Informacje diagnostyczne wypisywane do okna Output (czas budowy i zużycie pamięci).

## Jak zacząć

### Wymagania wstępne:

- Windows 10/11
- Visual Studio 2022 lub nowsze z obsługą WPF
- .NET 8 SDK

### Szybki start (Visual Studio):

1. Sklonuj repozytorium:

   git clone https://github.com/Znajka/M1.git
   cd M1

2. Otwórz rozwiązanie w Visual Studio (`.sln`) lub projekt (`.csproj`).
3. Upewnij się, że Target Framework w ustawieniach projektu to `.NET 8`.
4. Ustaw projekt `Tetrahedron_Sierpinski` jako startowy i uruchom (F5).

### Szybki start (dotnet CLI):

1. Przejdź do katalogu projektu zawierającego plik `.csproj`.
2. Uruchom:

   dotnet build
   dotnet run --project Tetrahedron_Sierpinski.csproj

(Uwaga: aplikacja to projekt WPF — uruchamianie z CLI działa tylko na Windows i gdy środowisko graficzne jest dostępne.)

### Sterowanie w aplikacji

- Lewy przycisk myszy + przeciąganie — ręczna rotacja modelu.
- Kółko myszy — przybliżanie/oddalanie kamery.
- Suwak "Poziom" — zmiana poziomu rekurencji fraktala.
- Pole "Max Level" — ustawienie maksymalnego poziomu (ograniczone w kodzie do 7 ze względów wydajnościowych).
- Przycisk "Start" — ustawia poziom na 0 i uruchamia automatyczne zwiększanie poziomu i obrót.
- Przycisk "Stop" — zatrzymuje automatyczną zmianę poziomu i obrót.

## Wydajność i ograniczenia

- Liczba tetraedrów rośnie wykładniczo z poziomem rekurencji — poziomy wyższe niż 6–7 mogą prowadzić do dużego zużycia pamięci i spadku wydajności.
- Kod obecnie ogranicza maksymalny poziom do 7. Możesz zmienić to ograniczenie w `MainWindow.xaml.cs`, ale zachowaj ostrożność.
- Aplikacja wypisuje metryki (czas budowy i RAM) do okna Output przy każdej rekonstrukcji modelu (użyj Visual Studio -> View -> Output).

## Gdzie uzyskać pomoc

- Zgłaszanie problemów i feature requestów: otwórz Issue w repozytorium.
- Dokumentacja i wytyczne dotyczące przyczyniania: plik `CONTRIBUTING.md` (jeśli istnieje) lub katalog `docs/`.
- Ogólne pytania dotyczące implementacji: sprawdź kod źródłowy w `MainWindow.xaml` i `MainWindow.xaml.cs`.

## Kto utrzymuje i jak kontrybuować

Projekt jest utrzymywany w repozytorium: `https://github.com/Znajka/M1` (gałąź `main`).

Jeśli chcesz wnieść wkład:

1. Przeczytaj `CONTRIBUTING.md` (jeśli dostępny).
2. Forkuj repozytorium i utwórz gałąź funkcji.
3. Wyślij Pull Request opisujący zmiany i uzasadnienie.

## Pliki powiązane

- `MainWindow.xaml` — UI i kontrolki.
- `MainWindow.xaml.cs` — logika budowy fraktala, obsługa wejścia i animacji.
- `CONTRIBUTING.md` — wytyczne dla kontrybutorów (jeśli istnieje).
- `LICENSE` — licencja projektu (zobacz plik w repozytorium).

---

Dziękuję za zainteresowanie projektem — zachęcam do eksperymentów z poziomami fraktala i optymalizacjami renderowania.