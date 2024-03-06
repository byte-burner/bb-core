import { useEffect, useState } from 'react';
import {
  getAvailableShells,
  disposeAll,
} from '../services';

export function useTerminalManager() {
  const [terminals, setTerminals] = useState([]);
  const [terminal, setTerminal] = useState(null);
  const [terminalTypes, setTerminalTypes] = useState([]);

  const add = async (term) => {
    await term.init();
    setTerminal(term);
    setTerminals([...terminals, term]);
  };

  const open = (term) => {
    term.open();
    setTerminal(term);
  };

  const remove = (term) => {
    let newTerm = null;
    if (term?.pid === terminal?.pid) {
      newTerm = terminals.find((t) => t.pid !== term?.pid);
      setTerminal(newTerm);
      setTerminals(terminals.filter((i) => i.pid !== term?.pid));
      term.destroy();
      newTerm?.open();
    } else {
      term.dispose();
      setTerminals(terminals.filter((i) => i.pid !== term?.pid));
    }
  };

  const removeAll = () => {
    // eslint-disable-next-line array-callback-return
    terminals.map((term) => {
      term.destroy();
    });
    setTerminal(null);
    setTerminals([]);
  };

  // cleanup open pty processes on window close
  window.addEventListener('beforeunload', () => {
    disposeAll();
  });

  useEffect(() => {
    const effectAsync = async () => {
      const types = await getAvailableShells();
      setTerminalTypes(types);
    };

    effectAsync();

    return () => {
      // cleanup open pty processes on component unmount
      disposeAll();
    };
  }, []);

  return {
    add,
    open,
    remove,
    removeAll,
    terminal,
    terminals,
    terminalTypes,
  };
}
