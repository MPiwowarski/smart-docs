// SPDX-License-Identifier: MIT
pragma solidity >=0.4.22 <0.9.0;

contract DocumentManager {
  string public object;

  constructor(string memory _object) public{
    object= _object;
  }

  function getObject() public view returns(string memory){
    return object;
  }

  function setObject(string memory _object) public {
    object = _object;
  }
}
