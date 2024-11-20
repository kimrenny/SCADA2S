document.addEventListener("DOMContentLoaded", function () {
  const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7045/sensor")
    .configureLogging(signalR.LogLevel.Information)
    .build();

  let unifiedChart;
  let unifiedChartData = {};

  async function start() {
    try {
      await connection.start();
      console.log("SignalR Connected.");
    } catch (err) {
      console.log(err);
      setTimeout(start, 5000);
    }
  }

  connection.on("receive", (id, value) => {
    console.log(`received value: ${value} for ID: ${id}`);
    const element = document.getElementById(id);
    if (element) {
      element.textContent = value;
      updateUnifiedChart(id, parseFloat(value));
    } else {
      console.warn(`Element with ID: ${id} not found.`);
    }
  });

  connection.on("receiveTime", (time) => {
    const parsedTime = new Date(time);
    const formattedTime = parsedTime.toLocaleString();
    console.log(`Received time: ${formattedTime}`);
    const timeElement = document.getElementById("current-time");
    if (timeElement) {
      timeElement.textContent = formattedTime;
    }
  });

  connection.onclose(async () => {
    await start();
  });

  start();

  document.getElementById("create-sensor-form").onsubmit = onFormSubmit;
  document.getElementById("update-sensor-form").onsubmit = onFormUpdate;

  getSensors();

  setInterval(() => {
    getSensors();
  }, 1000);

  document.getElementById("backgroundArea").onclick = (e) => {
    const xInput = document.getElementById("x");
    const yInput = document.getElementById("y");
    if (xInput && yInput) {
      xInput.value = e.offsetX;
      yInput.value = e.offsetY;
    }
  };

  function getSensors() {
    axios.get("https://localhost:7045/api/Sensor").then((result) => {
      result.data.forEach((sensor) => {
        if (!unifiedChartData[sensor.id]) {
          createUnifiedChartDataset(sensor.id, sensor.name);
        }
        updateUnifiedChart(sensor.id, parseFloat(sensor.value));
      });
    });
  }

  function onFormSubmit(e) {
    e.preventDefault();
    if (e.submitter.value === "Create") {
      tryNewSensor(
        e.target[0].value,
        e.target[1].value,
        e.target[2].value,
        e.target[3].value,
        e.target[4].value
      );
    }
  }

  function tryNewSensor(name, value, unit, x, y) {
    axios
      .post("https://localhost:7045/api/Sensor", {
        name: name,
        description: name,
        x,
        y,
        value: value,
        unit,
      })
      .then((result) => {
        if (!unifiedChartData[result.data]) {
          createUnifiedChartDataset(result.data, name);
        }
      });
  }

  function onFormUpdate(e) {
    e.preventDefault();
    if (e.submitter.value === "Update") {
      const param1 = e.target[0].value;
      const param2 = e.target[1].value;
      updateTargetValue(param1, param2);
    }
  }

  function changeBackgroundImage(id) {
    const element = document.getElementsByClassName("background-image")[0];
    element.src = "https://localhost:7045/api/BackgroundImage/" + id;
  }

  document
    .getElementById("changeBg")
    .addEventListener("submit", function (event) {
      event.preventDefault();
      const value = document.getElementById("changeBgNumber").value;
      changeBackgroundImage(value);
    });

  document
    .getElementById("imageForm")
    .addEventListener("submit", function (event) {
      event.preventDefault();

      const formData = new FormData();
      const fileInput = document.getElementById("imageInput").files[0];

      if (fileInput) {
        formData.append("image", fileInput);
        axios
          .post(
            "https://localhost:7045/api/backgroundimage/upload-image",
            formData
          )
          .then(() => {
            console.log("Image uploaded successfully");
          })
          .catch((error) => {
            console.error("Error : ", error);
          });
      } else {
        console.error("No file selected");
      }
    });

  function createUnifiedChart() {
    const chartContainer = document.createElement("div");
    chartContainer.style.marginTop = "20px";
    chartContainer.style.textAlign = "center";

    const chartCanvas = document.createElement("canvas");
    chartCanvas.id = "unified-chart";
    chartCanvas.style.width = "800px";
    chartCanvas.style.height = "400px";
    chartContainer.appendChild(chartCanvas);

    const form = document.getElementById("create-sensor-form");
    form.parentNode.insertBefore(chartContainer, form.nextSibling);

    unifiedChart = new Chart(chartCanvas.getContext("2d"), {
      type: "line",
      data: {
        labels: [],
        datasets: [],
      },
      options: {
        responsive: true,
        animation: false,
        maintainAspectRatio: false,
        scales: {
          x: { title: { display: true, text: "Time" } },
          y: {
            title: { display: true, text: "Value" },
            min: -100,
            max: 100,
          },
        },
      },
    });
  }

  function createUnifiedChartDataset(sensorId, sensorName) {
    if (!unifiedChart) {
      createUnifiedChart();
    }

    unifiedChartData[sensorId] = {
      labels: [],
      data: [],
    };

    unifiedChart.data.datasets.push({
      label: `Sensor: ${sensorName}`,
      data: unifiedChartData[sensorId].data,
      borderColor: getRandomColor(),
      backgroundColor: "rgba(0, 0, 0, 0)",
      borderWidth: 2,
    });
    unifiedChart.update();
  }

  function updateUnifiedChart(sensorId, value) {
    const currentTime = new Date().toLocaleTimeString();
    const history = unifiedChartData[sensorId];

    history.labels.push(currentTime);
    history.data.push(value);

    if (history.labels.length > 20) {
      history.labels.shift();
      history.data.shift();
    }

    unifiedChart.data.labels = history.labels;
    unifiedChart.update();
  }

  function getRandomColor() {
    return `rgba(${Math.floor(
      Math.random() * 255
    )}, ${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, 1)`;
  }
});
