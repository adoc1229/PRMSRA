-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 27, 2024 at 08:45 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `parkeasedatabase`
--

-- --------------------------------------------------------

--
-- Table structure for table `bookings`
--

CREATE TABLE `bookings` (
  `ID` int(11) NOT NULL,
  `UserID` text NOT NULL,
  `ParkingCode` text NOT NULL,
  `UserType` text NOT NULL,
  `VehicleType` text NOT NULL,
  `Status` text NOT NULL,
  `TicketNumber` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `employees`
--

CREATE TABLE `employees` (
  `ID` int(11) NOT NULL,
  `EmployeeID` text NOT NULL,
  `EmployeeName` text NOT NULL,
  `VehicleType` text NOT NULL,
  `PlateNumber` text NOT NULL,
  `ContactNumber` text NOT NULL,
  `Password` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `employees`
--

INSERT INTO `employees` (`ID`, `EmployeeID`, `EmployeeName`, `VehicleType`, `PlateNumber`, `ContactNumber`, `Password`) VALUES
(2, 'EMP317', 'Ryza', '4 Wheels', 'RYZ 015', '09923840694', 'RYZA123'),
(4, 'EMP686', 'Romeo', '4 Wheels', 'ROM 029', '09913830692', 'ROMEO123'),
(5, 'EMP338', 'Mark', '4 Wheels', 'QWD 144', '09912820695', 'MARK123');

-- --------------------------------------------------------

--
-- Table structure for table `parking`
--

CREATE TABLE `parking` (
  `ID` int(11) NOT NULL,
  `ParkingCode` text NOT NULL,
  `Status` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `parking`
--

INSERT INTO `parking` (`ID`, `ParkingCode`, `Status`) VALUES
(1, 'A11', 'AVAILABLE'),
(2, 'A12', 'AVAILABLE'),
(3, 'A21', 'AVAILABLE'),
(4, 'A22', 'AVAILABLE'),
(5, 'A23', 'AVAILABLE'),
(6, 'B11', 'AVAILABLE'),
(7, 'A33', 'AVAILABLE'),
(8, 'A31', 'AVAILABLE'),
(9, 'A32', 'AVAILABLE'),
(10, 'A34', 'AVAILABLE'),
(11, 'A41', 'AVAILABLE'),
(12, 'A43', 'AVAILABLE'),
(13, 'A44', 'AVAILABLE'),
(14, 'B42', 'AVAILABLE'),
(15, 'B12', 'AVAILABLE'),
(16, 'B21', 'AVAILABLE'),
(17, 'B22', 'AVAILABLE'),
(18, 'B23', 'AVAILABLE'),
(19, 'B31', 'AVAILABLE'),
(20, 'B32', 'AVAILABLE'),
(21, 'B33', 'AVAILABLE'),
(22, 'B34', 'AVAILABLE'),
(23, 'B41', 'AVAILABLE'),
(24, 'B43', 'AVAILABLE'),
(25, 'B44', 'AVAILABLE'),
(26, 'C11', 'AVAILABLE'),
(27, 'C12', 'AVAILABLE'),
(28, 'C21', 'AVAILABLE'),
(29, 'C22', 'AVAILABLE'),
(30, 'C23', 'AVAILABLE'),
(31, 'C31', 'AVAILABLE'),
(32, 'C32', 'AVAILABLE'),
(33, 'C33', 'AVAILABLE'),
(34, 'C34', 'AVAILABLE'),
(35, 'C41', 'AVAILABLE'),
(36, 'C42', 'AVAILABLE'),
(37, 'C43', 'AVAILABLE'),
(38, 'C44', 'AVAILABLE'),
(39, 'D11', 'AVAILABLE'),
(40, 'D12', 'AVAILABLE'),
(41, 'D21', 'AVAILABLE'),
(42, 'D22', 'AVAILABLE'),
(43, 'D23', 'AVAILABLE'),
(44, 'D31', 'AVAILABLE'),
(45, 'D32', 'AVAILABLE'),
(46, 'D33', 'AVAILABLE'),
(47, 'D34', 'AVAILABLE'),
(48, 'D41', 'AVAILABLE'),
(49, 'D42', 'AVAILABLE'),
(50, 'D43', 'AVAILABLE'),
(51, 'D44', 'AVAILABLE'),
(52, 'E11', 'AVAILABLE'),
(53, 'E12', 'AVAILABLE'),
(54, 'E21', 'AVAILABLE'),
(55, 'E22', 'AVAILABLE'),
(56, 'E23', 'AVAILABLE'),
(57, 'E31', 'AVAILABLE'),
(58, 'E32', 'AVAILABLE'),
(59, 'E33', 'AVAILABLE'),
(60, 'E34', 'AVAILABLE'),
(61, 'E41', 'AVAILABLE'),
(62, 'E42', 'AVAILABLE'),
(63, 'E43', 'AVAILABLE'),
(64, 'E44', 'AVAILABLE'),
(65, 'F11', 'AVAILABLE'),
(66, 'F12', 'AVAILABLE'),
(67, 'F21', 'AVAILABLE'),
(68, 'F22', 'AVAILABLE'),
(69, 'F23', 'AVAILABLE'),
(70, 'F31', 'AVAILABLE'),
(71, 'F32', 'AVAILABLE'),
(72, 'F33', 'AVAILABLE'),
(73, 'F34', 'AVAILABLE'),
(74, 'F41', 'AVAILABLE'),
(75, 'F42', 'AVAILABLE'),
(76, 'F43', 'AVAILABLE'),
(77, 'F44', 'AVAILABLE'),
(78, 'G11', 'AVAILABLE'),
(79, 'G12', 'AVAILABLE'),
(80, 'G21', 'AVAILABLE'),
(81, 'G22', 'AVAILABLE'),
(82, 'G23', 'AVAILABLE'),
(83, 'G31', 'AVAILABLE'),
(84, 'G32', 'AVAILABLE'),
(85, 'G33', 'AVAILABLE'),
(86, 'G34', 'AVAILABLE'),
(87, 'G41', 'AVAILABLE'),
(88, 'G42', 'AVAILABLE'),
(89, 'G43', 'AVAILABLE'),
(90, 'G44', 'AVAILABLE'),
(91, 'J11', 'AVAILABLE'),
(92, 'J12', 'AVAILABLE'),
(93, 'J13', 'AVAILABLE'),
(94, 'J21', 'AVAILABLE'),
(95, 'J22', 'AVAILABLE'),
(96, 'J23', 'AVAILABLE'),
(97, 'J31', 'AVAILABLE'),
(98, 'J32', 'AVAILABLE'),
(99, 'J33', 'AVAILABLE'),
(100, 'K11', 'AVAILABLE'),
(101, 'K12', 'AVAILABLE'),
(102, 'K13', 'AVAILABLE'),
(103, 'K21', 'AVAILABLE'),
(104, 'K22', 'AVAILABLE'),
(105, 'K23', 'AVAILABLE'),
(106, 'K31', 'AVAILABLE'),
(107, 'K32', 'AVAILABLE'),
(108, 'K33', 'AVAILABLE'),
(109, 'L11', 'AVAILABLE'),
(110, 'L12', 'AVAILABLE'),
(111, 'L13', 'AVAILABLE'),
(112, 'L21', 'AVAILABLE'),
(113, 'L22', 'AVAILABLE'),
(114, 'L23', 'AVAILABLE'),
(115, 'L31', 'AVAILABLE'),
(116, 'L32', 'AVAILABLE'),
(117, 'L33', 'AVAILABLE'),
(118, 'H11', 'AVAILABLE'),
(119, 'H12', 'AVAILABLE'),
(120, 'H13', 'AVAILABLE'),
(121, 'H21', 'AVAILABLE'),
(122, 'H22', 'AVAILABLE'),
(123, 'H23', 'AVAILABLE'),
(124, 'H31', 'AVAILABLE'),
(125, 'H32', 'AVAILABLE'),
(126, 'H33', 'AVAILABLE'),
(127, 'I11', 'AVAILABLE'),
(128, 'I12', 'AVAILABLE'),
(129, 'I13', 'AVAILABLE'),
(130, 'I21', 'AVAILABLE'),
(131, 'I22', 'AVAILABLE'),
(132, 'I23', 'AVAILABLE'),
(133, 'I31', 'AVAILABLE'),
(134, 'I32', 'AVAILABLE'),
(135, 'I33', 'AVAILABLE'),
(136, 'A42', 'AVAILABLE');

-- --------------------------------------------------------

--
-- Table structure for table `parking_log`
--

CREATE TABLE `parking_log` (
  `ID` int(11) NOT NULL,
  `TicketNumber` text NOT NULL,
  `TimeOut` text NOT NULL,
  `TimeIn` text NOT NULL,
  `ParkingFee` double NOT NULL,
  `ParkingDate` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `sales_report`
--

CREATE TABLE `sales_report` (
  `ID` int(11) NOT NULL,
  `ParkingDate` text NOT NULL,
  `TotalSales` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `ID` int(11) NOT NULL,
  `UserID` text NOT NULL,
  `VehicleOwner` text NOT NULL,
  `VehicleType` text NOT NULL,
  `PlateNumber` text NOT NULL,
  `Username` text NOT NULL,
  `Password` text NOT NULL,
  `ContactNumber` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`ID`, `UserID`, `VehicleOwner`, `VehicleType`, `PlateNumber`, `Username`, `Password`, `ContactNumber`) VALUES
(3, '59198', 'Romeo Adoc', '4 Wheels', 'ROM 409', 'adoc29', 'romeo123', '09913830692'),
(4, '67249', 'Edward Anderson', '4 Wheels', 'EDW143', 'Edward123', 'edward123', '09345819381'),
(5, '11073', 'Allan Genove', '4 Wheels', 'ALL 619', 'Allan123', 'allan123', '09385738291'),
(6, '29622', 'Romeo', '4 Wheels', 'ROM 229', 'adoc1229', 'qwer123', '09395609618');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bookings`
--
ALTER TABLE `bookings`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `employees`
--
ALTER TABLE `employees`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `parking`
--
ALTER TABLE `parking`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `parking_log`
--
ALTER TABLE `parking_log`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bookings`
--
ALTER TABLE `bookings`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;

--
-- AUTO_INCREMENT for table `employees`
--
ALTER TABLE `employees`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `parking`
--
ALTER TABLE `parking`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=137;

--
-- AUTO_INCREMENT for table `parking_log`
--
ALTER TABLE `parking_log`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
