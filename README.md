# SCADA Simulation System

This is a simple application to emulate the operation of a SCADA system. Users can:

- Create new sensors.
- Track data changes on sensors in real time via graphs.
- Upload own background images.
- Track the current time on the server.

## Installation

1.  **Clone the repository**:

    ```bash
    git clone https://github.com/kimrenny/SCADA2S.git
    ```

2.  **Navigate to the project directory**:

    ```bash
    cd SCADA2S
    ```

3. **Navigate to the server directory**:

    ```bash
    cd Server
    ```

4.  **Install the dependencies**:

    ```bash
    dotnet restore
    ```

5. **Navigate to the API directory**:

    ```bash
    cd CourseProject.API
    ```

6.  **Run the application**:

    ```bash
    dotnet run
    ```

    The app will be running at [https://localhost:7045/](https://localhost:7045/) and [http://localhost:5039/](http://localhost:5039/).

7. **Launch Visual Studio Code with the Live Server embedded in the UI directory.**

    The app will be running at [http://127.0.0.1:5500/](http://127.0.0.1:5500/).
