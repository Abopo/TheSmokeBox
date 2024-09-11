using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebServiceProjectManager : MonoBehaviour
{
    public static WebServiceProjectManager Instance;

    private readonly string _projectEndpoint = "https://bughunigamejam2024.azurewebsites.net/api/Projects";

    private const string ProjectsJSON = "[\r\n    {\r\n        \"projectID\": 16,\r\n        \"ownerName\": \"Abopo\",\r\n        \"name\": \"Ballin'\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-20T13:28:08.443\",\r\n        \"fileName\": \"Ballin'.json\"\r\n    },\r\n    {\r\n        \"projectID\": 17,\r\n        \"ownerName\": \"Abopo\",\r\n        \"name\": \"Dinoswim\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-20T13:42:46.583\",\r\n        \"fileName\": \"Dinoswim.json\"\r\n    },\r\n    {\r\n        \"projectID\": 18,\r\n        \"ownerName\": \"Abopo\",\r\n        \"name\": \"Crazy Star\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-20T13:56:52.673\",\r\n        \"fileName\": \"Crazy Star.json\"\r\n    },\r\n    {\r\n        \"projectID\": 23,\r\n        \"ownerName\": \"Lanceolate\",\r\n        \"name\": \"Stacks on stacks\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-20T20:08:11.06\",\r\n        \"fileName\": \"Stacks on stacks.json\"\r\n    },\r\n    {\r\n        \"projectID\": 24,\r\n        \"ownerName\": \"Lanceolate\",\r\n        \"name\": \"Funky Horns\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-20T20:19:40.62\",\r\n        \"fileName\": \"Funky Horns.json\"\r\n    },\r\n    {\r\n        \"projectID\": 25,\r\n        \"ownerName\": \"Lanceolate\",\r\n        \"name\": \"Animal Tower\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-20T20:38:26.777\",\r\n        \"fileName\": \"Animal Tower.json\"\r\n    },\r\n    {\r\n        \"projectID\": 35,\r\n        \"ownerName\": \"HyakuRiki\",\r\n        \"name\": \"a celebration\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T01:49:20.03\",\r\n        \"fileName\": \"a celebration.json\"\r\n    },\r\n    {\r\n        \"projectID\": 36,\r\n        \"ownerName\": \"HyakuRiki\",\r\n        \"name\": \"Hero\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T01:49:20.453\",\r\n        \"fileName\": \"Hero.json\"\r\n    },\r\n    {\r\n        \"projectID\": 37,\r\n        \"ownerName\": \"HyakuRiki\",\r\n        \"name\": \"how i feel\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T01:49:20.227\",\r\n        \"fileName\": \"how i feel.json\"\r\n    },\r\n    {\r\n        \"projectID\": 39,\r\n        \"ownerName\": \"Abopo\",\r\n        \"name\": \"Imbalance\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T01:54:23.943\",\r\n        \"fileName\": \"Imbalance.json\"\r\n    },\r\n    {\r\n        \"projectID\": 41,\r\n        \"ownerName\": \"Handstand\",\r\n        \"name\": \"Honk! Honk!\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T03:31:23.42\",\r\n        \"fileName\": \"Honk! Honk!.json\"\r\n    },\r\n    {\r\n        \"projectID\": 42,\r\n        \"ownerName\": \"Handstand\",\r\n        \"name\": \"Tallest Choochoo\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T04:22:20.163\",\r\n        \"fileName\": \"Tallest Choochoo.json\"\r\n    },\r\n    {\r\n        \"projectID\": 43,\r\n        \"ownerName\": \"Handstand\",\r\n        \"name\": \"Bowl-cut Bird\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T04:33:58.943\",\r\n        \"fileName\": \"Bowl-cut Bird.json\"\r\n    },\r\n    {\r\n        \"projectID\": 44,\r\n        \"ownerName\": \"Handstand\",\r\n        \"name\": \"Silly Little Beetle\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T05:11:00.48\",\r\n        \"fileName\": \"Silly Little Beetle.json\"\r\n    },\r\n    {\r\n        \"projectID\": 45,\r\n        \"ownerName\": \"Pityke\",\r\n        \"name\": \"abstract\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T21:02:19.59\",\r\n        \"fileName\": \"abstract#1.json\"\r\n    },\r\n    {\r\n        \"projectID\": 46,\r\n        \"ownerName\": \"Pityke\",\r\n        \"name\": \"secret\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T21:10:29.31\",\r\n        \"fileName\": \"secret.json\"\r\n    },\r\n    {\r\n        \"projectID\": 47,\r\n        \"ownerName\": \"Pityke\",\r\n        \"name\": \"Back to the roots\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T21:17:34.007\",\r\n        \"fileName\": \"Back to the roots.json\"\r\n    },\r\n    {\r\n        \"projectID\": 48,\r\n        \"ownerName\": \"Dog\",\r\n        \"name\": \"Spike\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T22:32:59.193\",\r\n        \"fileName\": \"Spike.json\"\r\n    },\r\n    {\r\n        \"projectID\": 49,\r\n        \"ownerName\": \"Dog\",\r\n        \"name\": \"Half Dollar Dino\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T22:42:48.45\",\r\n        \"fileName\": \"Half Dollar Dino.json\"\r\n    },\r\n    {\r\n        \"projectID\": 50,\r\n        \"ownerName\": \"Dog\",\r\n        \"name\": \"A Lovely Date\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T22:53:47.213\",\r\n        \"fileName\": \"A Lovely Date.json\"\r\n    },\r\n    {\r\n        \"projectID\": 51,\r\n        \"ownerName\": \"wujak\",\r\n        \"name\": \"graduating bird\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T23:15:24.48\",\r\n        \"fileName\": \"graduating bird.json\"\r\n    },\r\n    {\r\n        \"projectID\": 52,\r\n        \"ownerName\": \"wujak\",\r\n        \"name\": \"the beginning\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T23:31:14.697\",\r\n        \"fileName\": \"the beginning.json\"\r\n    },\r\n    {\r\n        \"projectID\": 53,\r\n        \"ownerName\": \"wujak\",\r\n        \"name\": \"just a neat bug\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-21T23:51:57.727\",\r\n        \"fileName\": \"just a neat bug.json\"\r\n    },\r\n    {\r\n        \"projectID\": 54,\r\n        \"ownerName\": \"Barrelbot\",\r\n        \"name\": \"Balance\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-22T17:15:30.36\",\r\n        \"fileName\": \"Balance.json\"\r\n    },\r\n    {\r\n        \"projectID\": 55,\r\n        \"ownerName\": \"Barrelbot\",\r\n        \"name\": \"They're Eyes\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-22T17:29:28.513\",\r\n        \"fileName\": \"They're Eyes.json\"\r\n    },\r\n    {\r\n        \"projectID\": 56,\r\n        \"ownerName\": \"Barrelbot\",\r\n        \"name\": \"Buy My Wares\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-22T17:42:30.627\",\r\n        \"fileName\": \"Buy My Wares.json\"\r\n    },\r\n    {\r\n        \"projectID\": 57,\r\n        \"ownerName\": \"Balbod\",\r\n        \"name\": \"Quadrupedal Clowning\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-24T16:11:47.353\",\r\n        \"fileName\": \"Quadrupedal Clowning.json\"\r\n    },\r\n    {\r\n        \"projectID\": 58,\r\n        \"ownerName\": \"Balbod\",\r\n        \"name\": \"How dinos used to live before man invented meteors\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-24T16:22:07.713\",\r\n        \"fileName\": \"How dinos used to live before man invented meteors.json\"\r\n    },\r\n    {\r\n        \"projectID\": 59,\r\n        \"ownerName\": \"Balbod\",\r\n        \"name\": \"Pandering to one of the judges\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-24T16:39:03.863\",\r\n        \"fileName\": \"Pandering to one of the judges.json\"\r\n    },\r\n    {\r\n        \"projectID\": 60,\r\n        \"ownerName\": \"Agent V\",\r\n        \"name\": \"Doubts of my Stupid Chat\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T03:33:19.66\",\r\n        \"fileName\": \"Doubts of my Stupid Chat.json\"\r\n    },\r\n    {\r\n        \"projectID\": 61,\r\n        \"ownerName\": \"krahezed\",\r\n        \"name\": \"bee fooled by bubble wand\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T03:43:18.253\",\r\n        \"fileName\": \"bee fooled by bubble wand.json\"\r\n    },\r\n    {\r\n        \"projectID\": 62,\r\n        \"ownerName\": \"Agent V\",\r\n        \"name\": \"What Chat Will Never Understand\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T03:43:55.513\",\r\n        \"fileName\": \"What Chat Will Never Understand.json\"\r\n    },\r\n    {\r\n        \"projectID\": 63,\r\n        \"ownerName\": \"Agent V\",\r\n        \"name\": \"Recalcitrance\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T03:58:30.787\",\r\n        \"fileName\": \"Recalcitrance.json\"\r\n    },\r\n    {\r\n        \"projectID\": 64,\r\n        \"ownerName\": \"krahezed\",\r\n        \"name\": \"smoke box box\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T04:15:51.593\",\r\n        \"fileName\": \"smoke box box.json\"\r\n    },\r\n    {\r\n        \"projectID\": 65,\r\n        \"ownerName\": \"TurboChickenMan\",\r\n        \"name\": \"Earthly Cascade\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T05:01:52.41\",\r\n        \"fileName\": \"Earthly Cascade.json\"\r\n    },\r\n    {\r\n        \"projectID\": 66,\r\n        \"ownerName\": \"krahezed\",\r\n        \"name\": \"the smoke stack\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T05:05:11.977\",\r\n        \"fileName\": \"the smoke stack.json\"\r\n    },\r\n    {\r\n        \"projectID\": 67,\r\n        \"ownerName\": \"TurboChickenMan\",\r\n        \"name\": \"Sprouting Consistency\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T05:16:23.91\",\r\n        \"fileName\": \"Sprouting Consistency.json\"\r\n    },\r\n    {\r\n        \"projectID\": 68,\r\n        \"ownerName\": \"TurboChickenMan\",\r\n        \"name\": \"Faint Grasp\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T06:10:52.18\",\r\n        \"fileName\": \"Faint Grasp.json\"\r\n    },\r\n    {\r\n        \"projectID\": 69,\r\n        \"ownerName\": \"Nowis-337\",\r\n        \"name\": \"BBQ Chipp\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T10:15:55.833\",\r\n        \"fileName\": \"BBQ Chipp.json\"\r\n    },\r\n    {\r\n        \"projectID\": 70,\r\n        \"ownerName\": \"Nowis-337\",\r\n        \"name\": \"Long Long Piggy Stick\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T10:44:08.7\",\r\n        \"fileName\": \"Long Long Piggy Stick.json\"\r\n    },\r\n    {\r\n        \"projectID\": 71,\r\n        \"ownerName\": \"Nowis-337\",\r\n        \"name\": \"Master of the Pit\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T11:25:11.82\",\r\n        \"fileName\": \"Master of the Pit.json\"\r\n    },\r\n    {\r\n        \"projectID\": 72,\r\n        \"ownerName\": \"Edwin\",\r\n        \"name\": \"Cylinder Steve\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T15:02:27.71\",\r\n        \"fileName\": \"Cylinder Steve.json\"\r\n    },\r\n    {\r\n        \"projectID\": 73,\r\n        \"ownerName\": \"Edwin\",\r\n        \"name\": \"Baby Chipp\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T15:14:29.113\",\r\n        \"fileName\": \"Baby Chipp.json\"\r\n    },\r\n    {\r\n        \"projectID\": 74,\r\n        \"ownerName\": \"Mayor McFrumples\",\r\n        \"name\": \"Justice?\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T20:00:26.533\",\r\n        \"fileName\": \"Justice?.json\"\r\n    },\r\n    {\r\n        \"projectID\": 75,\r\n        \"ownerName\": \"Mayor McFrumples\",\r\n        \"name\": \"What would you know of class?\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T20:21:35.977\",\r\n        \"fileName\": \"What would you know of class?.json\"\r\n    },\r\n    {\r\n        \"projectID\": 76,\r\n        \"ownerName\": \"Mayor McFrumples\",\r\n        \"name\": \"Gamers Only\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-25T20:44:48.143\",\r\n        \"fileName\": \"Gamers Only.json\"\r\n    },\r\n    {\r\n        \"projectID\": 77,\r\n        \"ownerName\": \"Jim_Jam_Slam\",\r\n        \"name\": \"Drill Truck\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-26T11:03:24.203\",\r\n        \"fileName\": \"Drill Truck.json\"\r\n    },\r\n    {\r\n        \"projectID\": 78,\r\n        \"ownerName\": \"Jim_Jam_Slam\",\r\n        \"name\": \"Bench\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-26T11:21:07.237\",\r\n        \"fileName\": \"Bench.json\"\r\n    },\r\n    {\r\n        \"projectID\": 79,\r\n        \"ownerName\": \"Jim_Jam_Slam\",\r\n        \"name\": \"Plunckit\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-26T11:29:25.037\",\r\n        \"fileName\": \"Plunckit.json\"\r\n    },\r\n    {\r\n        \"projectID\": 80,\r\n        \"ownerName\": \"Jim_Jam_Slam\",\r\n        \"name\": \"Big Tail\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-26T11:45:18.443\",\r\n        \"fileName\": \"Big Tail.json\"\r\n    },\r\n    {\r\n        \"projectID\": 81,\r\n        \"ownerName\": \"jimsFriend\",\r\n        \"name\": \"long neck boy in hat with ball\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-28T04:14:04.837\",\r\n        \"fileName\": \"long neck boy in hat with ball.json\"\r\n    },\r\n    {\r\n        \"projectID\": 82,\r\n        \"ownerName\": \"AlexanderArts\",\r\n        \"name\": \"Butterfly\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-29T04:08:57.91\",\r\n        \"fileName\": \"Butterfly.json\"\r\n    },\r\n    {\r\n        \"projectID\": 83,\r\n        \"ownerName\": \"AlexanderArts\",\r\n        \"name\": \"Gnome\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-29T04:21:35.347\",\r\n        \"fileName\": \"Gnome.json\"\r\n    },\r\n    {\r\n        \"projectID\": 84,\r\n        \"ownerName\": \"AlexanderArts\",\r\n        \"name\": \"World Turtle\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-29T04:35:40\",\r\n        \"fileName\": \"World Turtle.json\"\r\n    },\r\n    {\r\n        \"projectID\": 85,\r\n        \"ownerName\": \"Abopo\",\r\n        \"name\": \"Trash\",\r\n        \"isStreamSafe\": false,\r\n        \"lastModified\": \"2024-08-29T13:05:03.22\",\r\n        \"fileName\": \"Trash.json\"\r\n    },\r\n    {\r\n        \"projectID\": 86,\r\n        \"ownerName\": \"Abopo\",\r\n        \"name\": \"A Lovely Time\",\r\n        \"isStreamSafe\": false,\r\n        \"lastModified\": \"2024-08-29T13:05:04.437\",\r\n        \"fileName\": \"A Lovely Time.json\"\r\n    },\r\n    {\r\n        \"projectID\": 87,\r\n        \"ownerName\": \"QuietMen\",\r\n        \"name\": \"Wheels of Wood\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-30T14:30:01.247\",\r\n        \"fileName\": \"Wheels of Wood.json\"\r\n    },\r\n    {\r\n        \"projectID\": 88,\r\n        \"ownerName\": \"QuietMen\",\r\n        \"name\": \"Space Super Man\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-30T15:09:49.247\",\r\n        \"fileName\": \"Space Super Man.json\"\r\n    },\r\n    {\r\n        \"projectID\": 89,\r\n        \"ownerName\": \"QuietMen\",\r\n        \"name\": \"Treasure\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-08-30T15:30:26.883\",\r\n        \"fileName\": \"Treasure.json\"\r\n    },\r\n    {\r\n        \"projectID\": 90,\r\n        \"ownerName\": \"NatStarlight\",\r\n        \"name\": \"Cozy Forest Cottage\",\r\n        \"isStreamSafe\": false,\r\n        \"lastModified\": \"2024-08-31T08:06:17.837\",\r\n        \"fileName\": \"Cozy Forest Cottage.json\"\r\n    },\r\n    {\r\n        \"projectID\": 91,\r\n        \"ownerName\": \"NatStarlight\",\r\n        \"name\": \"Dino Buds\",\r\n        \"isStreamSafe\": false,\r\n        \"lastModified\": \"2024-08-31T08:32:12.843\",\r\n        \"fileName\": \"Dino Buds.json\"\r\n    },\r\n    {\r\n        \"projectID\": 92,\r\n        \"ownerName\": \"StarmanSeth\",\r\n        \"name\": \"Neo Armstrong Jet Cannon\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-09-01T02:12:03.763\",\r\n        \"fileName\": \"Neo Armstrong Jet Cannon .json\"\r\n    },\r\n    {\r\n        \"projectID\": 93,\r\n        \"ownerName\": \"StarmanSeth\",\r\n        \"name\": \"An Ode To Nessie\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-09-01T02:20:36.36\",\r\n        \"fileName\": \"An Ode To Nessie.json\"\r\n    },\r\n    {\r\n        \"projectID\": 94,\r\n        \"ownerName\": \"StarmanSeth\",\r\n        \"name\": \"But That Is Okay\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-09-01T02:30:16.907\",\r\n        \"fileName\": \"But That Is Okay.json\"\r\n    },\r\n    {\r\n        \"projectID\": 95,\r\n        \"ownerName\": \"NatStarlight\",\r\n        \"name\": \"Star Tree\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-09-02T23:21:43.13\",\r\n        \"fileName\": \"Star Tree.json\"\r\n    },\r\n    {\r\n        \"projectID\": 96,\r\n        \"ownerName\": \"NatStarlight\",\r\n        \"name\": \"Bulby Tree\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-09-03T22:13:03.677\",\r\n        \"fileName\": \"Bulby Tree.json\"\r\n    },\r\n    {\r\n        \"projectID\": 97,\r\n        \"ownerName\": \"NatStarlight\",\r\n        \"name\": \"Dino Graze\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-09-03T22:38:31.543\",\r\n        \"fileName\": \"Dino Graze.json\"\r\n    },\r\n    {\r\n        \"projectID\": 98,\r\n        \"ownerName\": \"NatStarlight\",\r\n        \"name\": \"Lovely Aquarium\",\r\n        \"isStreamSafe\": true,\r\n        \"lastModified\": \"2024-09-03T23:25:30.84\",\r\n        \"fileName\": \"Lovely Aquarium.json\"\r\n    }\r\n]";

    private List<Project> _projects;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        _projects = JsonConvert.DeserializeObject<List<Project>>(ProjectsJSON);
    }

    public void UploadProjectFile(string ownerName, string name, string path, Action<Project> callback, Action<string> handleFailure)
    {
        string serversDown = "Sorry, the server has been shut down!";
        handleFailure(serversDown);

        //if (!File.Exists(path))
        //{
        //    handleFailure.Invoke("File path error.");
        //    return;
        //}

        //byte[] data = new byte[0];
        //try
        //{
        //    data = File.ReadAllBytes(Path.GetFullPath(path));
        //}
        //catch (Exception e)
        //{
        //    handleFailure.Invoke(e.ToString());
        //    return;
        //}

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormFileSection("file", data, name + ".json", "file"));

        //string url = _projectEndpoint + "?userName=" + ownerName + "&projectName=" + name;

        //try
        //{
        //    StartCoroutine(WebRequestUtil.PostRequestForm<Project>(url, formData, callback, handleFailure));
        //}
        //catch (Exception e)
        //{
        //    handleFailure.Invoke(e.ToString());
        //}
    }

    public void GetProjects(Action<List<Project>> handleProjectsResponse, Action<string> handleFailure)
    {
        string url = _projectEndpoint;

        handleProjectsResponse.Invoke(_projects);

        //StartCoroutine(WebRequestUtil.GetRequest<List<Project>>(url, handleProjectsResponse, handleFailure));
    }

    // here im giving back a string, but you could probably change that
    // out for whatever data your serializing and get the class back
    public void GetProjectFile(int id, Action<SubmissionData> handleSuccess, Action<string> handleFailure)
    {
        string url = _projectEndpoint + "/" + id;

        // TODO: load the file based on the id
        var project = _projects.Find(x => x.ProjectID == id);
        if (project != null)
        {
            string path = "GalleryFiles/" + project.OwnerName + "-" + project.Name;
            path = path.Replace("?", "_");

            StartCoroutine(HandleLoadRequest(path, handleSuccess, handleFailure));
            return;
        }

        handleFailure("File not found");

        //StartCoroutine(WebRequestUtil.GetRequest<SubmissionData>(url, handleSuccess, handleFailure));
    }

    private IEnumerator HandleLoadRequest(string path, Action<SubmissionData> handleSuccess, Action<string> handleFailure)
    {
        ResourceRequest request = Resources.LoadAsync<TextAsset>(path);
        while (!request.isDone)
        {
            yield return null;
        }
        if (request.asset == null)
        {
            handleFailure("File not found");
        }
        TextAsset file = request.asset as TextAsset;

        if (file == null)
        {
            handleFailure("File not found");
        }

        var submissionData = JsonConvert.DeserializeObject<SubmissionData>(file.text);
        handleSuccess(submissionData);

        yield return request;
    }
}
