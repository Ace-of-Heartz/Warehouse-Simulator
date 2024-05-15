# ELTE Warehouse Simulator

---


|                  |                                                                                                                                                                                                                                      |
|------------------|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| Production CI/CD |             [![pipeline status](https://szofttech.inf.elte.hu/szofttech-ab-2024/group-02/csapat2/badges/production/pipeline.svg)](https://szofttech.inf.elte.hu/szofttech-ab-2024/group-02/csapat2/-/commits/production)             |
| Linux Build      |                               [![Linux Build](https://img.shields.io/badge/linux-0078D6?&logo=linux&logoColor=white&label=build&color=#FCC624)](https://trello.com/b/VtemS1SO/software-technology-bs)                                |
| Windows Build    |                                   [![Windows Build](https://img.shields.io/badge/windows-0078D6?&logo=windows&logoColor=white&label=build)](https://trello.com/b/VtemS1SO/software-technology-bs)                                    |
| MacOS Build      |                                     [![MacOS Build](https://img.shields.io/badge/mac--os-000000?&logo=apple&logoColor=white&label=build)](https://trello.com/b/VtemS1SO/software-technology-bs)                                      |
| WebGL Build      |    [![WebGL Build](https://img.shields.io/badge/web--gl-%23990000?&logo=webgl&logoColor=%23990000&logoSize=auto&label=build)](https://csapat2-szofttech-ab-2024-group-02-f878cb4f1fb257c1f8d29daf2918.szofttech.gitlab-pages.hu)     |

---


### Description

A warehouse simulation software made using Unity's framework. Simulate the path-finding of robots to their respective goals and log all events, for example collision errors, that might occur during the simulation.   \
**List of features:**
- Simulate the path planning of robots using an arbitrary layout of the warehouse, number of robots and number of goals.
- Use predefined path-finding algorithms for running multiple simulations.
- Configure the number of steps, planning time and preparation for more precise control over the simulation.
- Record and save the events during the simulation.
- Playback a past simulation with more careful controls for analyzing the sequence of events.

---

## The User Interface

<img src="docs/pics/mainmenu.png" alt="Playback" width=33%>
<img src="docs/pics/simulation.png" alt="Playback" width=33%  >
<img src="docs/pics/playback.png" alt="Playback" width=66%>


---

### How to run program:
1. Use the above links to download the build that's needed for your purposes.
2. Place the files in your chosen path.
3. Run the executable file.


### How to run __Simulation__:
1. Click on the Config File Location input field and search for a valid config file.
2. Enter the desired parameters for the simulation.
3. Click on the Event Log Location input field and search for a valid location for saving the event log created after the simulation is over.
4. Pick the desired search algorithm for the robots to use.
5. __Start Simulation__

### How to run __Playback__:
1. Click on the Map File Location input field and search for a valid map file.
2. Click on the Event Log Location input field and search for a valid event log file.

---

### Authors and acknowledgment:
Main and only contributors to this project:
- Szabó-Mayer "Blaaa" András
- Gálig Gergő
- Ferenci Ákos 

---

### Roadmap & Issues:

For the development of this project we have decided to use our own Trello board for managing issues and sharing the roadmap ahead of us.  
[![Trello](https://img.shields.io/badge/Trello-0052CC?style=for-the-badge&logo=trello&logoColor=white)](https://trello.com/b/VtemS1SO/software-technology-bs)


