import React, { useEffect, useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import { Link } from "react-router-dom";
import { listWinServices } from "../actions/winServiceAction";
import Loading from "../components/Loader";

import { HubConnection } from "signalr-client-react";
import * as signalR from "@aspnet/signalr";

function OverView(props) {
  const category = props.match.params.id ? props.match.params.id : "";
  const winServiceList = useSelector((state) => state.winServiceList);
  const { winService, loading, error } = winServiceList;
  const [searchKeyword, setSearchKeyword] = useState("");
  const [test, setTest] = useState();
  const [sortOrder, setSortOrder] = useState("");
  const dispatch = useDispatch();

  useEffect(() => {
    let connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:44306/signalRmessage")
      .build();

    dispatch(listWinServices(category));

    connection.on("send", (data) => {
      console.log("Log av  sendte data: " + data);
      dispatch(listWinServices(category));
    });
    connection
      .start()
      .then(() => console.log("Koblet til SignalR"))
      .catch((err) =>
        console.log("Error while starting SignalR connection: " + err)
      );

    return () => {};
  }, [test]);

  const submitHandler = (e) => {
    e.preventDefault();
    dispatch(listWinServices(category));
  };
  const sortHandler = (e) => {
    //setSortOrder(e.target.value);
    //dispatch(listProducts(category, searchKeyword, sortOrder));
  };
  const translateCategory = (category) => {
    if (category === "transfleet") {
      return "TransFleet";
    } else if (category === "tdxlog") {
      return "Tdx log";
    } else if (category === "lognett") {
      return "LogNett";
    }
  };
  const runningOrNot = (tf, index) => {
    if (tf.status === "Running") {
      return (
        <li key={index} className="running">
          {" "}
          {tf.displayName + "   "}Running{" "}
        </li>
      );
    } else {
      return (
        <li key={index} className="stopped">
          {" "}
          {tf.displayName + "   "}Stopped{" "}
        </li>
      );
    }
  };

  const runningOrNotVol2 = (service) => {
    let check = true;
    for (var i = 0; i < service; i++) {
      if (service.status !== "Running") {
        check = false;
      }
      console.log("Log av check :" + check);
      if (!check) {
        console.log("");
        return "className = stopped";
      } else {
        return "className = running";
      }
    }
  };

  return (
    <div>
      {category && (
        <h2 className="categories">{translateCategory(category)}</h2>
      )}

      {loading ? ( //Loading må være med her siden det er asynk og man få undifined feil på map funksjonen
        <div className="loading">
          {" "}
          <Loading />
        </div>
      ) : error ? (
        <div>{error}</div>
      ) : (
        <ul>
          {winService.map((service, index) => (
            <li key={index}>
              <div >
                <Link to={"/tms/" + service.tms.id}> {service.tms.name}</Link>
              </div>
              <li>{service.tms.category}</li>
              <ul>
                {service.tF_Services.map((tf, index) =>
                  runningOrNot(tf, index)
                )}
              </ul>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
export default OverView;
/* const runningOrNotVol2 = (tF_Services) => {
    let check =true;
    for (var i = 0; i < tF_Services; i++) {
      if (tF_Services.status !== "Running") {
        check = false;
      }
      if (!check) {
        return <div  className="stopped">
                <Link to={"/tms/" + service.tms.id}> {service.tms.name}</Link>
              </div>
              <li>{service.tms.category}</li>;
      } else {
        return  <div  className="running">
                <Link to={"/tms/" + service.tms.id}> {service.tms.name}</Link>
              </div>
              <li>{service.tms.category}</li>
      }
    }
  };*/
