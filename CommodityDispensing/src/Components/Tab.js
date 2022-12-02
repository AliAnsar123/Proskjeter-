import React, { Component } from "react";
import PropTypes from "prop-types";

class Tabs extends Component {
  static propTypes = {
    activeTab: PropTypes.string.isRequired,
    label: PropTypes.string.isRequired,
    onClick: PropTypes.func.isRequired,
  };

  onClick = () => {
    const { label, onClick } = this.props;
    onClick(label);
  };

  render() {
    const {
      onClick,
      props: { activeTab, label },
    } = this;

    let className = "tab-list-item";

    if (activeTab === label) {
      className += " tab-list-active";
    }

    return (
      <li
        tabIndex={0}
        className={className}
        onClick={onClick}
        onKeyDown={(e) => {
          if (e.key === "Enter") {
            onClick(label);
          }
        }}
      >
        {label}
      </li>
    );
  }
}

export default Tabs;
