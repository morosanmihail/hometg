import Sidebar from "../Sidebar";

export default function Header() {
    return (
        <header>
            <Sidebar/>
            <nav id="main-navbar" className="navbar navbar-expand-lg navbar-light bg-white fixed-top">
                <div className="container-fluid">
                    <a className="navbar-brand" href="/">
                        HomeTG
                    </a>
                </div>
            </nav>
        </header>
    );
}
